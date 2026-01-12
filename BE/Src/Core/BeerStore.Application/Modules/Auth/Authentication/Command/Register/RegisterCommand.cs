using BeerStore.Application.DTOs.Auth.Authentication.Responses.Register;
using BeerStore.Application.DTOs.Auth.User.Requests;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.Register
{
    public record RegisterCommand(CreateUserRequest Request) : IRequest<RegisterResponse>
    {
    }
}
