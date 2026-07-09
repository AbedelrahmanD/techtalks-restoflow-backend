using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Helpers;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxImageBytes = 2 * 1024 * 1024; // 2 MB

        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories.FindAsync(id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Category> CreateAsync(Category category, IFormFile? imageFile = null)
        {
            if (imageFile != null)
            {
                category.ImageUrl = await SaveImageAsync(imageFile);
            }

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category, IFormFile? imageFile = null)
        {
            if (imageFile != null)
            {
                category.ImageUrl = await SaveImageAsync(imageFile);
            }

            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }

        private static async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Categories");
            var saved = await FileHelper.SaveFileAsync(imageFile, uploadsRoot, AllowedImageExtensions, MaxImageBytes);
            return Path.Combine("Uploads", "Categories", saved).Replace('\\', '/');
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.Categories.FindAsync(id);
            if (existing == null)
            {
                return;
            }

            _db.Categories.Remove(existing);
            await _db.SaveChangesAsync();
        }
    }
}
