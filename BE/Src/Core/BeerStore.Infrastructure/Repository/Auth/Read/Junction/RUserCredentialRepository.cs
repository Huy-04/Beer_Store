using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read.Junction;
using Infrastructure.Core.Repository;
using BeerStore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read.Junction
{
    public class RUserCredentialRepository : ReadRepositoryGeneric<UserCredential>, IRUserCredentialRepository
    {
        public RUserCredentialRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<UserCredential?> GetByProviderAsync(Guid userId, string providerKey, CancellationToken token = default)
        {
            return await _context.Set<UserCredential>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProviderKey == providerKey, token);
        }
    }
}
