using BeerStore.Domain.Entities.Shop;
using BeerStore.Domain.IRepository.Shop.Write;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Shop.Write
{
    public class WStoreRepository : WriteRepositoryGeneric<Store>, IWStoreRepository
    {
        public WStoreRepository(ShopDbContext context) : base(context)
        {
        }
    }
}
