using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.IRepository.Shop.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Shop.Read
{
    public class RStoreAddressRepository : ReadRepositoryGeneric<StoreAddress>, IRStoreAddressRepository
    {
        public RStoreAddressRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StoreAddress>> GetByStoreIdAsync(Guid storeId, CancellationToken token)
        {
            return await FindAsync(a => a.StoreId == storeId, token);
        }
    }
}
