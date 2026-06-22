using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user, string plainPassword);
        Task UpdateAsync(User user, string? plainPassword = null);
        Task DeleteAsync(int id);
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByEmailAsync(string email);
    }
}
