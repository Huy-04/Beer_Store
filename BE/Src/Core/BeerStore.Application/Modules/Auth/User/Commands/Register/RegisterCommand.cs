using BeerStore.Application.DTOs.Auth.User.Requests;
using BeerStore.Application.DTOs.Auth.User.Responses.Register;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Commands.Register
{
    public record RegisterCommand(CreateUserRequest Request) : IRequest<RegisterResponse>
    {
    }
}