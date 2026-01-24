using BeerStore.Domain.Entities.Auth;
using Domain.Core.Interface.IRepository;
using Domain.Core.ValueObjects.Address;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRUserAddressRepository : IReadRepositoryGeneric<UserAddress>
    {
        Task<IEnumerable<UserAddress>> GetByUserIdAsync(Guid userId, CancellationToken token);
    }
}
