using Microsoft.AspNetCore.Http;
using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<MenuItem?> GetByNameAsync(string name);
        Task<MenuItem> CreateAsync(MenuItem menuItem, IFormFile? imageFile = null);
        Task UpdateAsync(MenuItem menuItem, IFormFile? imageFile = null);
        Task DeleteAsync(int id);
    }
}
