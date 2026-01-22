using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Auth.Read.Junction
{
    public class RUserRoleRepository : ReadRepositoryGeneric<UserRole>, IRUserRoleRepository
    {
        public RUserRoleRepository(AuthDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken token = default)
        {
            return AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, token);
        }
    }
}
