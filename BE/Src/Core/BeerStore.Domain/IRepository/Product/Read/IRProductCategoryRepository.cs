using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Read
{
    public interface IRProductCategoryRepository : IReadRepositoryGeneric<Entities.Product.ProductCategory>
    {
        Task<IEnumerable<Entities.Product.ProductCategory>> GetByProductIdAsync(Guid productId, CancellationToken token);

        Task<IEnumerable<Entities.Product.ProductCategory>> GetByCategoryIdAsync(Guid categoryId, CancellationToken token);

        Task<Entities.Product.ProductCategory?> GetPrimaryByProductIdAsync(Guid productId, CancellationToken token);
    }
}
