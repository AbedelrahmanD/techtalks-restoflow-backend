using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using System.Linq;

namespace RestoFlow.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Order> CreateAsync(Order order)
        {
             order.CreatedAt = DateTime.UtcNow;

            // Set unit prices from menu items and calculate total amount
            if (order.Items != null && order.Items.Any())
            {
                foreach (var item in order.Items)
                {
                    var menu = await _db.MenuItems.FindAsync(item.MenuItemId);
                    item.UnitPrice = menu?.Price ?? 0m;
                }

                order.TotalAmount = order.Items.Sum(item => item.UnitPrice * item.Quantity);
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

             return await GetByIdAsync(order.Id) ?? order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _db.Orders
                .AsNoTracking()
                .Include(order => order.Table)
                .Include(order => order.Items!)
                    .ThenInclude(i => i.MenuItem)
                .OrderBy(order => order.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _db.Orders
                .AsNoTracking()
                .Include(order => order.Table)
                .Include(order => order.Items!)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<Order?> GetActiveByTableIdAsync(int tableId)
        {
            return await _db.Orders
                .AsNoTracking()
                .Include(order => order.Table)
                .Include(order => order.Items!)
                    .ThenInclude(i => i.MenuItem)
                .Where(order => order.TableId == tableId && order.Status != "Paid" && order.Status != "Voided")
                .OrderBy(order => order.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
