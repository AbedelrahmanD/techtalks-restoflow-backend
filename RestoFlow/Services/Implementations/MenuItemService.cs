using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Helpers;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxImageBytes = 2 * 1024 * 1024; // 2 MB

        private readonly AppDbContext _db;

        public MenuItemService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _db.MenuItems
                .Include(m => m.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _db.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MenuItem> CreateAsync(MenuItem menuItem, IFormFile? imageFile = null)
        {
            if (imageFile != null)
            {
                menuItem.ImageUrl = await SaveImageAsync(imageFile);
            }

            _db.MenuItems.Add(menuItem);
            await _db.SaveChangesAsync();
            return menuItem;
        }

        public async Task UpdateAsync(MenuItem menuItem, IFormFile? imageFile = null)
        {
            if (imageFile != null)
            {
                menuItem.ImageUrl = await SaveImageAsync(imageFile);
            }

            _db.MenuItems.Update(menuItem);
            await _db.SaveChangesAsync();
        }

        private static async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "MenuItems");
            var saved = await FileHelper.SaveFileAsync(imageFile, uploadsRoot, AllowedImageExtensions, MaxImageBytes);
            return Path.Combine("Uploads", "MenuItems", saved).Replace('\\', '/');
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.MenuItems.FindAsync(id);
            if (existing == null)
            {
                return;
            }

            _db.MenuItems.Remove(existing);
            await _db.SaveChangesAsync();
        }
    }
}
