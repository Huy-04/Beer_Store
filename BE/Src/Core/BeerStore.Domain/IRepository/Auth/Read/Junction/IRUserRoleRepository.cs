using BeerStore.Domain.Entities.Auth.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read.Junction
{
    public interface IRUserRoleRepository : IReadRepositoryGeneric<UserRole>
    {
        Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken token = default);
    }
}
