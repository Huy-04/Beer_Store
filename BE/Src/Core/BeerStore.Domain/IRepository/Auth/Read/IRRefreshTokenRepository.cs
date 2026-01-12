using BeerStore.Domain.Entities.Auth;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRRefreshTokenRepository : IReadRepositoryGeneric<RefreshToken>
    {
        Task<RefreshToken?> FindTokenAsync(CancellationToken token = default, string? tokenHash = null, Guid? userId = null, string? deviceId = null);

        Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken token = default);

        Task<RefreshToken?> GetByUserIdAndDeviceIdAsync(Guid userId, string deviceId, CancellationToken token = default);

        Task<IEnumerable<RefreshToken>> GetAllActiveAsync(CancellationToken token = default);
    }
}