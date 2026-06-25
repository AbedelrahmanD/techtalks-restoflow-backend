using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IDiningTableService
    {
        Task<IEnumerable<DiningTable>> GetAllAsync();
        Task<DiningTable?> GetByIdAsync(int id);
        Task<DiningTable> CreateAsync(DiningTable table);
        Task UpdateAsync(DiningTable table);
        Task DeleteAsync(int id);
    }
}
