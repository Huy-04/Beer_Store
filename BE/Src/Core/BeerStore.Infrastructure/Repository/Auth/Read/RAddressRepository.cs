using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RAddressRepository : ReadRepositoryGeneric<Address>, IRAddressRepository
    {
        public RAddressRepository(AuthDbContext context):base(context) { }

    }
}
