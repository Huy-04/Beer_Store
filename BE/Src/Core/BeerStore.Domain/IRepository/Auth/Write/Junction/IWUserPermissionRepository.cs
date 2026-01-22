using BeerStore.Domain.Entities.Auth.Junction;
using Domain.Core.Interface.IRepository;

namespace BeerStore.Domain.IRepository.Auth.Write.Junction
{
    public interface IWUserPermissionRepository : IWriteRepositoryGeneric<UserPermission>
    {
    }
}
