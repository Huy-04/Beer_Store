using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.ValueObjects.Auth.Role;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RRoleRepository : ReadRepositoryGeneric<Role>, IRRoleRepository
    {
        public RRoleRepository(AuthDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsByNameAsync(RoleName roleName, CancellationToken token = default, Guid? idRole = null)
        {
            return AnyAsync(r => r.RoleName == roleName && (idRole == null || r.Id != idRole), token);
        }

        public Task<Role?> GetByNameAsync(RoleName roleName, CancellationToken token = default)
        {
            return _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleName == roleName, token);
        }

        public async Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<Guid> roleIds, CancellationToken token = default)
        {
            return await FindAsync(r => roleIds.Contains(r.Id), token);
        }
    }
}
