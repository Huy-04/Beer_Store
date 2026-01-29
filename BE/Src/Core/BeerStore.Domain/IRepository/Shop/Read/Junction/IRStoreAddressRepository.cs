using BeerStore.Domain.Entities.Shop.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Shop.Read.Junction
{
    public interface IRStoreAddressRepository : IReadRepositoryGeneric<StoreAddress>
    {
        Task<IEnumerable<StoreAddress>> GetByStoreIdAsync(Guid storeId, CancellationToken token = default);
    }
}
