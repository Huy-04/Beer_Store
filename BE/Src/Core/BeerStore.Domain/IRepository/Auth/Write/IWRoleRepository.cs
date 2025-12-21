using BeerStore.Domain.Entities.Auth;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth
{
    public interface IWRoleRepository : IWriteRepositoryGeneric<Role>
    {
    }
}