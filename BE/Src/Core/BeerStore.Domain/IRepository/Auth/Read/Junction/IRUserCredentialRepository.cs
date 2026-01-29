using BeerStore.Domain.Entities.Auth.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read.Junction
{
    public interface IRUserCredentialRepository : IReadRepositoryGeneric<UserCredential>
    {
        Task<UserCredential?> GetByProviderAsync(Guid userId, string providerKey, CancellationToken token = default);
    }
}
