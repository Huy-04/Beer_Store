using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRAddressRepository : IReadRepositoryGeneric<Address>
    {
    }
}
