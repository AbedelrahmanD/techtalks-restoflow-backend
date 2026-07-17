using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using System.Linq;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly ISettingService _settingService;
        private readonly AppDbContext _db;

        public MenuService(ISettingService settingService, AppDbContext db)
        {
            _settingService = settingService;
            _db = db;
        }

        public async Task<MenuResultDto> GetMenuAsync()
        {
            var settings = await _settingService.GetAsync();

            var settingsDto = new SettingResponseDto
            {
                Currency = settings.Currency,
                RestaurantName = settings.RestaurantName,
                LogoUrl = settings.LogoUrl,

            };

            var categories = await _db.Categories
                .Include(category => category.MenuItems.Where(menuItem => menuItem.IsActive))
                .AsNoTracking()
                .Where(category => category.IsActive)
                .ToListAsync();

            var menu = categories.Select(category => new MenuCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.ImageUrl,
                Items = category.MenuItems
                    .Select(menuItem => new MenuItemSimpleDto
                    {
                        Id = menuItem.Id,
                        Name = menuItem.Name,
                        Price = menuItem.Price,
                        Image = menuItem.ImageUrl
                    }).ToList()
            }).ToList();

            return new MenuResultDto
            {
                Settings = settingsDto,
                Menu = menu
            };
        }
    }
}
