using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.ValueObjects.Auth.Permission;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RPermissionRepository : ReadRepositoryGeneric<Permission>, IRPermissionRepository
    {
        public RPermissionRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(PermissionName permissionName, CancellationToken token = default, Guid? idPermission = null)
        {
            return await AnyAsync(r => r.PermissionName == permissionName && (idPermission == null || r.Id != idPermission), token);
        }
    }
}
