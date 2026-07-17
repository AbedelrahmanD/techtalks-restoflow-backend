using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _db;
        private readonly IFileService _fileService;

        public SettingService(AppDbContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        public async Task<Setting> GetAsync()
        {
            var settings = await _db.Settings.AsNoTracking()
                .Include(settings => settings.Currency)
                .FirstOrDefaultAsync();

            if (settings is null)
            {
                settings = new Setting();
                settings.Currency = new Currency();

            }

            return settings;
        }

        public async Task<Setting> SaveAsync(Setting setting, IFormFile? logoFile = null)
        {
            // If logo file provided, save it and set LogoUrl
            if (logoFile != null)
            {
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Settings");
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var maxBytes = 2 * 1024 * 1024; // 2 MB
                var saved = await _fileService.SaveFileAsync(logoFile, uploadsRoot, allowed, maxBytes);
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
                // If a new logo was uploaded and an old logo exists, delete the old file
                if (!string.IsNullOrEmpty(setting.LogoUrl) && !string.IsNullOrEmpty(existing.LogoUrl))
                {
                    await _fileService.RemoveFileAsync(existing.LogoUrl);
                }

                existing.CurrencyId = setting.CurrencyId;
                existing.RestaurantName = setting.RestaurantName;
                if (!string.IsNullOrEmpty(setting.LogoUrl))
                {
                    existing.LogoUrl = setting.LogoUrl;
                }
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            return await GetAsync();
        }
    }
}
