using BeerStore.Application.DTOs.Auth.User.Requests;
using BeerStore.Application.DTOs.Auth.User.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Commands.CreateUser
{
    public record CreateUserCommand(Guid CreatedBy, Guid UpdatedBy, CreateUserRequest Request) : IRequest<UserResponse>
    {
    }
}