using Microsoft.AspNetCore.Http;
using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByNameAsync(string name);
        Task<Category> CreateAsync(Category category, IFormFile? imageFile = null);
        Task UpdateAsync(Category category, IFormFile? imageFile = null);
        Task DeleteAsync(int id);
    }
}
