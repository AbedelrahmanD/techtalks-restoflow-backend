using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetActiveByTableIdAsync(int tableId);
        Task<Order> CreateAsync(Order order);
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}
