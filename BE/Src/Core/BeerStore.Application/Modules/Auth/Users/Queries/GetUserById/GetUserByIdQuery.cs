using BeerStore.Application.DTOs.Auth.User.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid IdUser) : IRequest<UserResponse>
    {
    }
}
