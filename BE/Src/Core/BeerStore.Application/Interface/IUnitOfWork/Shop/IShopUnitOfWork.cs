using Application.Core.Interface.IUnitOfWork;
using BeerStore.Domain.IRepository.Shop.Read;
using BeerStore.Domain.IRepository.Shop.Write;
using BeerStore.Domain.IRepository.Shop.Read.Junction;

namespace BeerStore.Application.Interface.IUnitOfWork.Shop
{
    public interface IShopUnitOfWork : IUnitOfWorkGeneric
    {
        // Store
        IRStoreRepository RStoreRepository { get; }

        IWStoreRepository WStoreRepository { get; }

        // StoreAddress
        IRStoreAddressRepository RStoreAddressRepository { get; }
    }
}
