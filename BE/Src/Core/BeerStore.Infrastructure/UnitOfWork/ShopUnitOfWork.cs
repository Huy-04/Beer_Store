using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Domain.IRepository.Shop.Read;
using BeerStore.Domain.IRepository.Shop.Write;
using BeerStore.Infrastructure.Persistence.Db;
using BeerStore.Infrastructure.Repository.Shop.Read;
using BeerStore.Infrastructure.Repository.Shop.Write;
using Infrastructure.Core.UnitOfWork;

namespace BeerStore.Infrastructure.UnitOfWork
{
    public class ShopUnitOfWork : UnitOfWorkGeneric, IShopUnitOfWork
    {
        public ShopUnitOfWork(ShopDbContext context) : base(context)
        {
            // Store
            RStoreRepository = new RStoreRepository(context);
            WStoreRepository = new WStoreRepository(context);

            // StoreAddress
            RStoreAddressRepository = new RStoreAddressRepository(context);
            WStoreAddressRepository = new WStoreAddressRepository(context);
        }

        public IRStoreRepository RStoreRepository { get; }
        public IWStoreRepository WStoreRepository { get; }
        public IRStoreAddressRepository RStoreAddressRepository { get; }
        public IWStoreAddressRepository WStoreAddressRepository { get; }
    }
}
