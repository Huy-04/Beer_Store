using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.User.Responses
{
    public record UserResponse(
        Guid IdUser,
        string Email,
        string Phone,
        string FullName,
        string UserName,
        string Password,
        StatusEnum UserStatus,
        StatusEnum EmailStatus,
        StatusEnum PhoneStatus,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}