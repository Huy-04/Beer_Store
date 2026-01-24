using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Read
{
    public interface IRProductVariantRepository : IReadRepositoryGeneric<Entities.Product.ProductVariant>
    {
        Task<IEnumerable<Entities.Product.ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken token);

        Task<Entities.Product.ProductVariant?> GetBySkuAsync(Guid storeId, string sku, CancellationToken token);

        Task<bool> ExistsBySkuAsync(Guid storeId, string sku, CancellationToken token);
    }
}
