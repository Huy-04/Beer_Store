using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write
{
    public class WPermissionRepository : WriteRepositoryGeneric<Permission>, IWPermissionRepository
    {
        public WPermissionRepository(AuthDbContext context) : base(context)
        {
        }
    }
}