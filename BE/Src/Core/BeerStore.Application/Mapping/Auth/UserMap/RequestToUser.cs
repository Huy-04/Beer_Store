using BeerStore.Application.DTOs.Auth.User.Requests;
using Application.Core.Interface.Services;
using BeerStore.Application.Interface.Services;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.User;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using Domain.Core.ValueObjects.Address;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Application.Mapping.Auth.UserMap
{
    public static class RequestToUser
    {
        public static User ToUser(this CreateUserRequest request, IPasswordHasher passwordHasher, Guid CreatedBy, Guid UpdatedBy)
        {
            var passwordHash = passwordHasher.HashPassword(request.Password);

            return User.Create(
                Email.Create(request.Email),
                Phone.Create(request.Phone),
                FullName.Create(request.FullName),
                UserName.Create(request.UserName),
                Password.Create(passwordHash),
                CreatedBy,
                UpdatedBy);
        }

        public static void ApplyUser(this User user, IPasswordHasher passwordHasher, UpdateUserRequest request, Guid UpdatedBy)
        {
            var passwordHash = passwordHasher.HashPassword(request.Password);

            user.UpdateEmail(Email.Create(request.Email));
            user.UpdatePhone(Phone.Create(request.Phone));
            user.UpdateFullName(FullName.Create(request.FullName));
            user.UpdateUserName(UserName.Create(request.UserName));
            user.UpdatePassword(Password.Create(passwordHash));
            user.UpdateUserStatus(UserStatus.Create(request.UserStatus));
            user.UpdateEmailStatus(EmailStatus.Create(request.EmailStatus));
            user.UpdatePhoneStatus(PhoneStatus.Create(request.PhoneStatus));
            user.SetUpdatedBy(UpdatedBy);
        }
    }
}
