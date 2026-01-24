using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Read
{
    public interface IRCategoryRepository : IReadRepositoryGeneric<Entities.Product.Category>
    {
        Task<Entities.Product.Category?> GetBySlugAsync(string slug, CancellationToken token);

        Task<IEnumerable<Entities.Product.Category>> GetByParentIdAsync(Guid? parentId, CancellationToken token);

        Task<IEnumerable<Entities.Product.Category>> GetRootCategoriesAsync(CancellationToken token);

        Task<IEnumerable<Entities.Product.Category>> GetActiveTreeAsync(CancellationToken token);

        Task<bool> ExistsBySlugAsync(string slug, CancellationToken token);
    }
}
