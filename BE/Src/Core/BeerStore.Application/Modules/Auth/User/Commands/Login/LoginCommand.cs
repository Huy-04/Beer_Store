using BeerStore.Application.DTOs.Auth.User.Requests.Login;
using BeerStore.Application.DTOs.Auth.User.Responses.Login;
using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Commands.Login
{
    public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>
    {
    }
}
