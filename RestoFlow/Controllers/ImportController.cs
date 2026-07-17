using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using System.Text.Json;
public class MealDbResponse
{
    public List<MealItemDto>? Meals { get; set; }
}

public class MealItemDto
{
    public string IdMeal { get; set; } = string.Empty;
    public string strMeal { get; set; } = string.Empty;
    public string strCategory { get; set; } = string.Empty;
    public string? strInstructions { get; set; }
    public string? strMealThumb { get; set; } // Image URL
}

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly AppDbContext _db; // Replace with your actual DbContext class name
    private readonly HttpClient _httpClient;

    public ImportController(AppDbContext db, HttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    [HttpGet()]
    public async Task<IActionResult> ImportMeals()
    {
        // 1. Fetch the data from the external API
        var apiUrl = "https://www.themealdb.com/api/json/v1/1/search.php?f=a";
        var response = await _httpClient.GetAsync(apiUrl);
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Failed to fetch data from TheMealDB.");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var apiData = JsonSerializer.Deserialize<MealDbResponse>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (apiData?.Meals == null || !apiData.Meals.Any())
        {
            return NotFound("No meals found in the API response.");
        }

        // 2. Ensure directories exist for holding downloaded images
        var categoryFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Categories");
        var menuItemFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "MenuItems");

        Directory.CreateDirectory(categoryFolder);
        Directory.CreateDirectory(menuItemFolder);

        // Group the incoming meals by Category Name
        var groupedMeals = apiData.Meals.GroupBy(m => m.strCategory);

        foreach (var group in groupedMeals)
        {
            var categoryName = group.Key;
            var mealsInCategory = group.ToList();

            // 3. Prevent duplicate categories
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (category == null)
            {
                category = new Category
                {
                    Name = categoryName,
                    IsActive = true
                };
                _db.Categories.Add(category);
                await _db.SaveChangesAsync(); // Save to generate category.Id
            }

            string? categoryImageFilename = null;

            // 4. Process and save each meal item
            foreach (var meal in mealsInCategory)
            {
                // Prevent duplicate menu items
                var exists = await _db.MenuItems.AnyAsync(m => m.Name == meal.strMeal && m.CategoryId == category.Id);
                if (exists) continue;

                string? menuItemImageFilename = null;

                // Download Item Image
                if (!string.IsNullOrEmpty(meal.strMealThumb))
                {
                    menuItemImageFilename = await DownloadImageAsync(meal.strMealThumb, menuItemFolder);
                }

                // Keep track of the very first item's image to assign to the Category
                if (categoryImageFilename == null && menuItemImageFilename != null)
                {
                    // Copy the image file over to the Categories folder
                    var sourcePath = Path.Combine(menuItemFolder, menuItemImageFilename);
                    categoryImageFilename = Guid.NewGuid() + Path.GetExtension(menuItemImageFilename);
                    var destPath = Path.Combine(categoryFolder, categoryImageFilename);

                    System.IO.File.Copy(sourcePath, destPath, true);
                }

                var newItem = new MenuItem
                {
                    CategoryId = category.Id,
                    Name = meal.strMeal,
                    Description = meal.strInstructions,
                    Price = 12.99m, // Default generic price
                    ImageUrl = menuItemImageFilename != null ? $"/Uploads/MenuItems/{menuItemImageFilename}" : null,
                    IsActive = true
                };

                _db.MenuItems.Add(newItem);
            }

            // 5. Assign the Category image if we found one
            if (categoryImageFilename != null && string.IsNullOrEmpty(category.ImageUrl))
            {
                category.ImageUrl = $"/Uploads/Categories/{categoryImageFilename}";
                _db.Categories.Update(category);
            }

            await _db.SaveChangesAsync();
        }


        return Ok(new { 
            Message="Done, check api/menu api"
        });
    }

    // Helper method to safely download image streams
    private async Task<string?> DownloadImageAsync(string imageUrl, string destinationFolder)
    {
        try
        {
            var response = await _httpClient.GetAsync(imageUrl);
            if (!response.IsSuccessStatusCode) return null;

            var extension = ".jpg"; // Fallback extension
            var uri = new Uri(imageUrl);
            var pathSegment = uri.AbsolutePath;
            if (Path.HasExtension(pathSegment))
            {
                extension = Path.GetExtension(pathSegment);
            }

            var uniqueFilename = Guid.NewGuid().ToString() + extension;
            var fullPath = Path.Combine(destinationFolder, uniqueFilename);

            await using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fs);

            return uniqueFilename;
        }
        catch
        {
            return null; // Don't crash the entire loop if one image fails to download
        }
    }
}