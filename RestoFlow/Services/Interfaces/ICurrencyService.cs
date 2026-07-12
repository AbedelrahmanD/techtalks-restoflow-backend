using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllAsync();
        Task<Currency?> GetByIdAsync(int id);
        Task<Currency?> GetByCodeAsync(string code);
        Task<Currency> CreateAsync(Currency currency);
        Task UpdateAsync(Currency currency);
        Task DeleteAsync(int id);
    }
}
