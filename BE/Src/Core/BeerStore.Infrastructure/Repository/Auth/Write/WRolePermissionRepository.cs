using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write
{
    internal class WRolePermissionRepository : WriteRepositoryGeneric<RolePermission>, IWRolePermissionRepository
    {
        public WRolePermissionRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
