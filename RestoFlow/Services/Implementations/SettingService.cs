using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using RestoFlow.Helpers;

namespace RestoFlow.Services.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _db;

        public SettingService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Setting?> GetAsync()
        {
            return await _db.Settings.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Setting> SaveAsync(Setting setting, IFormFile? logoFile = null)
        {
            // If logo file provided, save it and set LogoUrl
            if (logoFile != null)
            {
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Settings");
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var maxBytes = 2 * 1024 * 1024; // 2 MB
                var saved = await FileHelper.SaveFileAsync(logoFile, uploadsRoot, allowed, maxBytes);
                if (!string.IsNullOrEmpty(saved))
                {
                    // store relative path
                    setting.LogoUrl = Path.Combine("Uploads", "Settings", saved).Replace('\\', '/');
                }
            }

            var existing = await _db.Settings.FirstOrDefaultAsync();
            if (existing == null)
            {
                setting.UpdatedAt = DateTime.UtcNow;
                _db.Settings.Add(setting);
            }
            else
            {
                existing.CurrencyId = setting.CurrencyId;
                existing.RestaurantName = setting.RestaurantName;
                if (!string.IsNullOrEmpty(setting.LogoUrl))
                {
                    existing.LogoUrl = setting.LogoUrl;
                }
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            return existing ?? setting;
        }
    }
}
