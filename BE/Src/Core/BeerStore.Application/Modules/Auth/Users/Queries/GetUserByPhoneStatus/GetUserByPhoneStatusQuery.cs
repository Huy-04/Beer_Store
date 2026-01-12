using BeerStore.Application.DTOs.Auth.User.Responses;
using Domain.Core.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhoneStatus
{
    public record GetUserByPhoneStatusQuery(StatusEnum PhoneStatus) : IRequest<IEnumerable<UserResponse>>
    {
    }
}
