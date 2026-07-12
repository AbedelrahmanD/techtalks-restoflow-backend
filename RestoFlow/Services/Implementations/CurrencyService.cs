using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly AppDbContext _db;

        public CurrencyService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _db.Currencies.AsNoTracking().ToListAsync();
        }

        public async Task<Currency?> GetByIdAsync(int id)
        {
            return await _db.Currencies.FindAsync(id);
        }

        public async Task<Currency?> GetByCodeAsync(string code)
        {
            return await _db.Currencies.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Currency> CreateAsync(Currency currency)
        {
            _db.Currencies.Add(currency);
            await _db.SaveChangesAsync();
            return currency;
        }

        public async Task UpdateAsync(Currency currency)
        {
            _db.Currencies.Update(currency);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.Currencies.FindAsync(id);
            if (existing == null) return;

            _db.Currencies.Remove(existing);
            await _db.SaveChangesAsync();
        }
    }
}
