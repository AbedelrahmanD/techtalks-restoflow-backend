using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _db;

        public RefreshTokenService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<RefreshToken> CreateAsync(int userId, TimeSpan? ttl = null)
        {
            var expires = DateTime.UtcNow.Add(ttl ?? TimeSpan.FromDays(7));
            var token = GenerateToken();

            var rt = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expires = expires,
                CreatedAt = DateTime.UtcNow,
                Revoked = false
            };

            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync();
            return rt;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _db.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task RevokeAsync(RefreshToken token, string? replacedBy = null)
        {
            token.Revoked = true;
            token.ReplacedByToken = replacedBy;
            await _db.SaveChangesAsync();
        }

        private static string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
