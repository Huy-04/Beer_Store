using BeerStore.Application.DTOs.Auth.User.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByUserName
{
    public record GetUserByUserNameQuery(string UserName) : IRequest<IEnumerable<UserResponse>>
    {
    }
}