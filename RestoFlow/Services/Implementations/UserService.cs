using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> CreateAsync(User user, string plainPassword)
        {
            user.CreatedAt = DateTime.UtcNow;
            // store bcrypt hashed password
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.Users.FindAsync(id);
            if (existing == null)
            {
                return;
            }

            _db.Users.Remove(existing);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == name);
        }

        public async Task UpdateAsync(User user, string? plainPassword = null)
        {
            if (!string.IsNullOrEmpty(plainPassword))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            }

            user.UpdatedAt = DateTime.UtcNow;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
