using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Read
{
    public interface IRProductRepository : IReadRepositoryGeneric<Entities.Product.Product>
    {
        Task<Entities.Product.Product?> GetBySlugAsync(Guid storeId, string slug, CancellationToken token);

        Task<IEnumerable<Entities.Product.Product>> GetByStoreIdAsync(Guid storeId, CancellationToken token);

        Task<bool> ExistsBySlugAsync(Guid storeId, string slug, CancellationToken token);

        Task<IEnumerable<Entities.Product.Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken token);

        Task<IEnumerable<Entities.Product.Product>> GetByBrandIdAsync(Guid brandId, CancellationToken token);
    }
}
