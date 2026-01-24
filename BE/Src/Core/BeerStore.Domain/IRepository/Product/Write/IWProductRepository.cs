using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Product.Write
{
    public interface IWProductRepository : IWriteRepositoryGeneric<Entities.Product.Product>
    {
    }
}
