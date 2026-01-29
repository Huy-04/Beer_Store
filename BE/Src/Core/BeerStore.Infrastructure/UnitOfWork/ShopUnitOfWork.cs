using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Domain.IRepository.Shop.Read;
using BeerStore.Domain.IRepository.Shop.Read.Junction;
using BeerStore.Domain.IRepository.Shop.Write;
using BeerStore.Infrastructure.Persistence.Db;

using Infrastructure.Core.UnitOfWork;

namespace BeerStore.Infrastructure.UnitOfWork
{
    public class ShopUnitOfWork : UnitOfWorkGeneric, IShopUnitOfWork
    {
        public ShopUnitOfWork(
            ShopDbContext context,
            IRStoreRepository rStoreRepo,
            IWStoreRepository wStoreRepo,
            IRStoreAddressRepository rStoreAddressRepo) : base(context)
        {
            RStoreRepository = rStoreRepo;
            WStoreRepository = wStoreRepo;

            RStoreAddressRepository = rStoreAddressRepo;
        }

        // Store
        public IRStoreRepository RStoreRepository { get; }

        public IWStoreRepository WStoreRepository { get; }

        // StoreAddress
        public IRStoreAddressRepository RStoreAddressRepository { get; }
    }
}
