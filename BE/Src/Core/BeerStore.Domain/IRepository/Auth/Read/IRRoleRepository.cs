using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Role;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRRoleRepository : IReadRepositoryGeneric<Role>
    {
        Task<bool> ExistsByNameAsync(RoleName roleName, CancellationToken token = default, Guid? idRole = null);

        Task<Role?> GetByNameAsync(RoleName roleName, CancellationToken token = default);

        Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<Guid> roleIds, CancellationToken token = default);
    }
}
