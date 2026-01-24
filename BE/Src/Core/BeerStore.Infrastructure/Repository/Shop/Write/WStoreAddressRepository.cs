using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.IRepository.Shop.Write;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Shop.Write
{
    public class WStoreAddressRepository : WriteRepositoryGeneric<StoreAddress>, IWStoreAddressRepository
    {
        public WStoreAddressRepository(ShopDbContext context) : base(context)
        {
        }
    }
}
