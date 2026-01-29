using BeerStore.Domain.Entities.Shop.Junction;
using BeerStore.Domain.IRepository.Shop.Read.Junction;
using Infrastructure.Core.Repository;
using BeerStore.Infrastructure.Persistence.Db;

namespace BeerStore.Infrastructure.Repository.Shop.Read.Junction
{
    public class RStoreAddressRepository : ReadRepositoryGeneric<StoreAddress>, IRStoreAddressRepository
    {
        public RStoreAddressRepository(ShopDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<StoreAddress>> GetByStoreIdAsync(Guid storeId, CancellationToken token = default)
        {
            return await FindAsync(x => x.StoreId == storeId, token);
        }
    }
}
