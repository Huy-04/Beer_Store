using BeerStore.Domain.Entities.Auth;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRRefreshTokenRepository : IReadRepositoryGeneric<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHash(string tokenHash, CancellationToken token = default);

        Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken token = default);

        Task<RefreshToken?> GetByUserIdAndDeviceIdAsync(Guid userId, string deviceId, CancellationToken token = default);

        Task<RefreshToken?> GetByTokenHashAndDeviceId(string tokenHash, string deviceId, CancellationToken token = default);

        Task<IEnumerable<RefreshToken>> GetAllActiveAsync(CancellationToken token = default);
    }
}