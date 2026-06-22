using RestoFlow.Models;
using Microsoft.AspNetCore.Http;

namespace RestoFlow.Services.Interfaces
{
    public interface ISettingService
    {
        Task<Setting?> GetAsync();
        Task<Setting> SaveAsync(Setting setting, IFormFile? logoFile = null);
    }
}
