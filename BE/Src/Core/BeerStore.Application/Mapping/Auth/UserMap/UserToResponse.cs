using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.UserMap
{
    public static class UserToResponse
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new(
                user.Id,
                user.Email.Value,
                user.Phone.Value,
                user.FullName.Value,
                user.UserName.Value,
                user.Password.Value,
                user.UserStatus.Value,
                user.EmailStatus.Value,
                user.PhoneStatus.Value,
                user.CreatedBy,
                user.UpdatedBy,
                user.CreatedAt,
                user.UpdatedAt);
        }
    }
}