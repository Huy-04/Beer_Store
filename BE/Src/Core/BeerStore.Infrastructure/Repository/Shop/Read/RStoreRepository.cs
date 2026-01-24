using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.IRepository.Shop.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Shop.Read
{
    public class RStoreRepository : ReadRepositoryGeneric<Store>, IRStoreRepository
    {
        public RStoreRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task<Store?> GetBySlugAsync(string slug, CancellationToken token)
        {
            return await _entities.FirstOrDefaultAsync(s => s.Slug.Value == slug, token);
        }

        public async Task<Store?> GetByOwnerIdAsync(Guid ownerId, CancellationToken token)
        {
            return await _entities.FirstOrDefaultAsync(s => s.OwnerId == ownerId, token);
        }

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken token)
        {
            return await AnyAsync(s => s.Slug.Value == slug, token);
        }
    }
}
