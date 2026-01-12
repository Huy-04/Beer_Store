using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Write;
using BeerStore.Infrastructure.Persistence.Db;
using Infrastructure.Core.IRepository;

namespace BeerStore.Infrastructure.Repository.Auth.Write
{
    public class WRefreshTokenRepository : WriteRepositoryGeneric<RefreshToken>, IWRefreshTokenRepository
    {
        public WRefreshTokenRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
