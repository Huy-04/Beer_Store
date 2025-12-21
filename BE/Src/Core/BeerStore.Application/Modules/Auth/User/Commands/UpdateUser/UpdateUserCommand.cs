using BeerStore.Application.DTOs.Auth.User.Requests;
using BeerStore.Application.DTOs.Auth.User.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Commands.UpdateUser
{
    public record UpdateUserCommand(Guid IdUser, Guid UpdatedBy, UpdateUserRequest Request) : IRequest<UserResponse>
    {
    }
}