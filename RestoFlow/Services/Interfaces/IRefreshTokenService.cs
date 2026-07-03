using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateAsync(int userId, TimeSpan? ttl = null);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task RevokeAsync(RefreshToken token, string? replacedBy = null);
    }
}
