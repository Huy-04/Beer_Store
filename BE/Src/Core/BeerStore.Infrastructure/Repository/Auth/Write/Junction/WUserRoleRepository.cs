using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Write.Junction;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write.Junction
{
    internal class WUserRoleRepository : WriteRepositoryGeneric<UserRole>, IWUserRoleRepository
    {
        public WUserRoleRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
