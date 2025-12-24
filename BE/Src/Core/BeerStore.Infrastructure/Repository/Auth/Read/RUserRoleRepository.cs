using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.Repository;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RUserRoleRepository : ReadRepositoryGeneric<UserRole>, IRUserRoleRepository
    {
        public RUserRoleRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
