using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Domain.ValueObjects.Auth.User;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery(string Email) : IRequest<IEnumerable<UserResponse>>
    {
    }
}
