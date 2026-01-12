using BeerStore.Domain.Entities.Auth.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read.Junction
{
    public interface IRRolePermissionRepository : IReadRepositoryGeneric<RolePermission>
    {
    }
}
