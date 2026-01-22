using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write
{
    internal class WUserAddressRepository : WriteRepositoryGeneric<UserAddress>, IWUserAddressRepository
    {
        public WUserAddressRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
