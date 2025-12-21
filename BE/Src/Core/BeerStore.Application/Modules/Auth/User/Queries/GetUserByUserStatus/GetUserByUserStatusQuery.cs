using BeerStore.Application.DTOs.Auth.User.Responses;
using Domain.Core.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByUserStatus
{
    public record GetUserByUserStatusQuery(StatusEnum UserStatus) : IRequest<IEnumerable<UserResponse>>
    {
    }
}