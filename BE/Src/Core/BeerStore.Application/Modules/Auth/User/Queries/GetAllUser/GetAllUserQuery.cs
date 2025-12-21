using BeerStore.Application.DTOs.Auth.User.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetAllUser
{
    public record GetAllUserQuery : IRequest<IEnumerable<UserResponse>>
    {
    }
}