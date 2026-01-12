using BeerStore.Application.DTOs.Auth.User.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhone
{
    public record GetUserByPhoneQuery(string Phone) : IRequest<IEnumerable<UserResponse>>
    {
    }
}
