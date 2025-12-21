using BeerStore.Domain.Entities.Auth;
using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.User.Requests
{
    public record UpdateUserRequest(
        string Email,
        string Phone,
        string FullName,
        string UserName,
        string Password,
        StatusEnum UserStatus,
        StatusEnum EmailStatus,
        StatusEnum PhoneStatus)
    {
    }
}