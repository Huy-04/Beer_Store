using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write
{
    public class WUserRepository : WriteRepositoryGeneric<User>, IWUserRepository
    {
        public WUserRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
