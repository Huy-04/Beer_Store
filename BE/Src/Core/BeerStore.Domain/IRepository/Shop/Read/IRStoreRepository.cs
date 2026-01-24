using BeerStore.Domain.Entities.Shop;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Shop.Read
{
    public interface IRStoreRepository : IReadRepositoryGeneric<Store>
    {
        Task<Store?> GetBySlugAsync(string slug, CancellationToken token);

        Task<Store?> GetByOwnerIdAsync(Guid ownerId, CancellationToken token);

        Task<bool> ExistsBySlugAsync(string slug, CancellationToken token);
    }
}