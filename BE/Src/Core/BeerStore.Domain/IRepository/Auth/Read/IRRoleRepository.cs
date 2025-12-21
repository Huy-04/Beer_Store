using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Roles;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRRoleRepository : IReadRepositoryGeneric<Role>
    {
        Task<bool> ExistsByNameAsync(RoleName roleName, CancellationToken token = default, Guid? idRole = null);

        public Task<Role?> GetByNameAsync(RoleName roleName, CancellationToken token = default);
    }
}