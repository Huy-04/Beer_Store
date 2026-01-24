using BeerStore.Domain.Entities.Shop;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Shop.Read
{
    public interface IRStoreAddressRepository : IReadRepositoryGeneric<StoreAddress>
    {
        Task<IEnumerable<StoreAddress>> GetByStoreIdAsync(Guid storeId, CancellationToken token);
    }
}