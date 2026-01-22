using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Auth.Read.Junction
{
    public class RUserPermissionRepository : ReadRepositoryGeneric<UserPermission>, IRUserPermissionRepository
    {
        public RUserPermissionRepository(AuthDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsAsync(Guid userId, Guid permissionId, CancellationToken token = default)
        {
            return AnyAsync(up => up.UserId == userId && up.PermissionId == permissionId, token);
        }
    }
}
