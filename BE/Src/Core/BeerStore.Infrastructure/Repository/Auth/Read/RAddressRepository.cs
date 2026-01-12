using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RAddressRepository : ReadRepositoryGeneric<Address>, IRAddressRepository
    {
        public RAddressRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId, CancellationToken token)
        {
            return await FindAsync(a => a.UserId == userId, token);
        }
    }
}