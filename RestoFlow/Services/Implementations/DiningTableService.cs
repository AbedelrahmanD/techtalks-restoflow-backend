using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class DiningTableService : IDiningTableService
    {
        private readonly AppDbContext _db;

        public DiningTableService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DiningTable>> GetAllAsync()
        {
            return await _db.Tables.AsNoTracking().ToListAsync();
        }

        public async Task<DiningTable?> GetByIdAsync(int id)
        {
            return await _db.Tables.FindAsync(id);
        }

        public async Task<DiningTable?> GetByQrCodeTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            return await _db.Tables.FirstOrDefaultAsync(t => t.QrCodeToken == token);
        }

        public async Task<DiningTable> CreateAsync(DiningTable table)
        {
           
            table.QrCodeToken = Guid.NewGuid().ToString();
            _db.Tables.Add(table);
            await _db.SaveChangesAsync();
            return table;
        }

        public async Task UpdateAsync(DiningTable table)
        {
            
            _db.Tables.Update(table);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.Tables.FindAsync(id);
            if (existing == null) return;
            _db.Tables.Remove(existing);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> TableNumberExistsAsync(string tableNumber, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(tableNumber))
                return false;

            var normalized = tableNumber.Trim().ToLower();

            var query = _db.Tables.AsNoTracking()
                .Where(t => t.TableNumber.Trim().ToLower() == normalized);

            if (excludeId.HasValue)
                query = query.Where(t => t.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
