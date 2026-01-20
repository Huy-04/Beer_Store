using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.ValueObjects.Auth.Permission;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RPermissionRepository : ReadRepositoryGeneric<Permission>, IRPermissionRepository
    {
        private readonly AuthDbContext _authContext;

        public RPermissionRepository(AuthDbContext context) : base(context)
        {
            _authContext = context;
        }

        public async Task<bool> ExistsByNameAsync(PermissionName permissionName, CancellationToken token = default, Guid? idPermission = null)
        {
            return await AnyAsync(r => r.PermissionName == permissionName && (idPermission == null || r.Id != idPermission), token);
        }

        public async Task<IEnumerable<string>> GetPermissionNamesByRoleIdsAsync(IEnumerable<Guid> roleIds, CancellationToken token = default)
        {
            return await _authContext.Set<RolePermission>()
                .AsNoTracking()
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Join(_entities,
                    rp => rp.PermissionId,
                    p => p.Id,
                    (rp, p) => p.PermissionName.Value)
                .Distinct()
                .ToListAsync(token);
        }
    }
}