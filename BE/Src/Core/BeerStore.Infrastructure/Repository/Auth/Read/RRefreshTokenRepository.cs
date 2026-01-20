using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RRefreshTokenRepository : ReadRepositoryGeneric<RefreshToken>, IRRefreshTokenRepository
    {
        public RRefreshTokenRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetByTokenHash(string tokenHash, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, token);
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return await FindAsync(
                rt => rt.UserId == userId && rt.TokenStatus == TokenStatus.Active && rt.ExpiresAt > DateTimeOffset.UtcNow,
                token);
        }

        public async Task<RefreshToken?> GetByTokenHashAndDeviceId(string tokenHash, string deviceId, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash && rt.DeviceId == deviceId && rt.ExpiresAt > DateTimeOffset.UtcNow, token);
        }

        public async Task<RefreshToken?> GetByUserIdAndDeviceIdAsync(Guid userId, string deviceId, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.DeviceId == deviceId
                && rt.TokenStatus == TokenStatus.Active && rt.ExpiresAt > DateTimeOffset.UtcNow, token);
        }

        public async Task<IEnumerable<RefreshToken>> GetAllActiveAsync(CancellationToken token = default)
        {
            return await FindAsync(
                rt => rt.TokenStatus == TokenStatus.Active && rt.ExpiresAt > DateTimeOffset.UtcNow,
                token);
        }
    }
}