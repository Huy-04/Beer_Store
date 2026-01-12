using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Permission;
using BeerStore.Domain.ValueObjects.Auth.Permission.Enums;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRPermissionRepository : IReadRepositoryGeneric<Permission>
    {
        Task<bool> ExistsByNameAsync(PermissionName permissionName, CancellationToken token = default, Guid? idPermission = null);
    }
}
