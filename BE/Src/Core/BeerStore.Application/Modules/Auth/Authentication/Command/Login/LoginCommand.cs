using BeerStore.Application.DTOs.Auth.Authentication.Requests.Login;
using BeerStore.Application.DTOs.Auth.Authentication.Responses.Login;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.Login
{
    public record LoginCommand(
        string DeviceId,
        string DeviceName,
        string IpAddress,
        LoginRequest Request) : IRequest<LoginResponse>;
}