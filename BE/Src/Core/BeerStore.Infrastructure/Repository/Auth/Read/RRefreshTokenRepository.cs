using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Domain.Core.Enums;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RRefreshTokenRepository : ReadRepositoryGeneric<RefreshToken>, IRRefreshTokenRepository
    {
        public RRefreshTokenRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> FindTokenAsync(CancellationToken token = default, string? tokenHash = null, Guid? userId = null, string? deviceId = null)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(rt =>
                    (tokenHash == null || rt.TokenHash.Value == tokenHash) &&
                    (userId == null || rt.UserId == userId) &&
                    (deviceId == null || rt.DeviceId.Value == deviceId),
                    token);
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return await FindAsync(
                rt => rt.UserId == userId && rt.TokenStatus.Value == StatusEnum.Active && rt.ExpiresAt > DateTimeOffset.UtcNow,
                token);
        }

        public async Task<RefreshToken?> GetByUserIdAndDeviceIdAsync(Guid userId, string deviceId, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.DeviceId.Value == deviceId, token);
        }

        public async Task<IEnumerable<RefreshToken>> GetAllActiveAsync(CancellationToken token = default)
        {
            return await FindAsync(
                rt => rt.TokenStatus.Value == StatusEnum.Active && rt.ExpiresAt > DateTimeOffset.UtcNow,
                token);
        }
    }
}