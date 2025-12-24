using BeerStore.Domain.Entities.Auth.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Write
{
    public interface IWRolePermissionRepository : IWriteRepositoryGeneric<RolePermission>
    {
    }
}
