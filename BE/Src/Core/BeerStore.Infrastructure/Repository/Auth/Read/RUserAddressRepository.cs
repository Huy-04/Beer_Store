using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RUserAddressRepository : ReadRepositoryGeneric<UserAddress>, IRUserAddressRepository
    {
        public RUserAddressRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
