using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Permissions;
using BeerStore.Domain.ValueObjects.Auth.Permissions.Enums;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRPermissionRepository : IReadRepositoryGeneric<Permission>
    {
        Task<bool> ExistsByNameAsync(PermissionName permissionName, CancellationToken token = default, Guid? idPermission = null);
    }
}