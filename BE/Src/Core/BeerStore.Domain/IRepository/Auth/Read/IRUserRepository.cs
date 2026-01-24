using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.Interface.IRepository;
using Domain.Core.ValueObjects.Address;

namespace BeerStore.Domain.IRepository.Auth.Read
{
    public interface IRUserRepository : IReadRepositoryGeneric<User>
    {
        Task<bool> ExistsByUserNameAsync(UserName userName, CancellationToken token = default, Guid? idUser = null);

        Task<bool> ExistsByPhoneAsync(Phone phone, CancellationToken token = default, Guid? idUser = null);

        Task<bool> ExistsByEmailAsync(Email email, CancellationToken token = default, Guid? idUser = null);

        Task<User?> GetByUserNameAsync(UserName userName, CancellationToken token = default);

        Task<User?> GetByEmailWithRolesAsync(Email email, CancellationToken token = default);

        Task<User?> GetByIdWithRolesAsync(Guid userId, CancellationToken token = default);
    }
}
