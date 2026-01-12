using BeerStore.Application.DTOs.Auth.User.Responses;
using Domain.Core.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetByUserByEmailStatus
{
    public record GetUserByEmailStatusQuery(StatusEnum EmailStatus) : IRequest<IEnumerable<UserResponse>>
    {
    }
}
