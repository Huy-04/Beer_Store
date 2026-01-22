using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Write.Junction;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write.Junction
{
    internal class WUserPermissionRepository : WriteRepositoryGeneric<UserPermission>, IWUserPermissionRepository
    {
        public WUserPermissionRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
