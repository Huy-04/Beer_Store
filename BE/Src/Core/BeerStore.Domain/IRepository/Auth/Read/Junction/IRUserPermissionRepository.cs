using BeerStore.Domain.Entities.Auth.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read.Junction
{
    public interface IRUserPermissionRepository : IReadRepositoryGeneric<UserPermission>
    {
        Task<bool> ExistsAsync(Guid userId, Guid permissionId, CancellationToken token = default);
    }
}
