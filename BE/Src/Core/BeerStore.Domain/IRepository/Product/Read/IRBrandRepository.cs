using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Read
{
    public interface IRBrandRepository : IReadRepositoryGeneric<Entities.Product.Brand>
    {
        Task<Entities.Product.Brand?> GetBySlugAsync(string slug, CancellationToken token);

        Task<IEnumerable<Entities.Product.Brand>> GetActiveAsync(CancellationToken token);

        Task<IEnumerable<Entities.Product.Brand>> GetVerifiedAsync(CancellationToken token);

        Task<bool> ExistsBySlugAsync(string slug, CancellationToken token);
    }
}
