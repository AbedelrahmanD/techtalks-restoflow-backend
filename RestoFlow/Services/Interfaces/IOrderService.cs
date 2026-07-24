using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}
