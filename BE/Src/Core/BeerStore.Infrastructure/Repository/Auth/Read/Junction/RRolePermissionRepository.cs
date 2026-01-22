using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Auth.Read.Junction
{
    public class RRolePermissionRepository : ReadRepositoryGeneric<RolePermission>, IRRolePermissionRepository
    {
        public RRolePermissionRepository(AuthDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken token = default)
        {
            return AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, token);
        }
    }
}
