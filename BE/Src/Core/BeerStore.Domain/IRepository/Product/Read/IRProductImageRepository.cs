using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Read
{
    public interface IRProductImageRepository : IReadRepositoryGeneric<Entities.Product.ProductImage>
    {
        Task<IEnumerable<Entities.Product.ProductImage>> GetByProductIdAsync(Guid productId, CancellationToken token);

        Task<IEnumerable<Entities.Product.ProductImage>> GetByVariantIdAsync(Guid variantId, CancellationToken token);

        Task<Entities.Product.ProductImage?> GetPrimaryByProductIdAsync(Guid productId, CancellationToken token);
    }
}
