using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.IRepository.Auth.Read;
using BeerStore.Domain.ValueObjects.Auth.User;
using BeerStore.Infrastructure.Persistence.Db;
using Domain.Core.ValueObjects.Address;
using Domain.Core.ValueObjects.Common;
using Infrastructure.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeerStore.Infrastructure.Repository.Auth.Read
{
    public class RUserRepository : ReadRepositoryGeneric<User>, IRUserRepository
    {
        public RUserRepository(AuthDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsByUserNameAsync(UserName userName, CancellationToken token = default, Guid? idUser = null)
        {
            return AnyAsync(u => u.UserName == userName && (idUser == null || u.Id != idUser), token);
        }

        public Task<bool> ExistsByPhoneAsync(Phone phone, CancellationToken token = default, Guid? idUser = null)
        {
            return AnyAsync(u => u.Phone == phone && (idUser == null || u.Id != idUser), token);
        }

        public Task<bool> ExistsByEmailAsync(Email email, CancellationToken token = default, Guid? idUser = null)
        {
            return AnyAsync(u => u.Email == email && (idUser == null || u.Id != idUser), token);
        }

        public async Task<User?> GetByUserNameAsync(UserName userName, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName, token);
        }

        public async Task<User?> GetByEmailWithRolesAsync(Email email, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == email, token);
        }

        public async Task<User?> GetByIdWithRolesAsync(Guid userId, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId, token);
        }

        public async Task<User?> GetByIdWithAddressesAsync(Guid userId, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.Id == userId, token);
        }
    }
}
