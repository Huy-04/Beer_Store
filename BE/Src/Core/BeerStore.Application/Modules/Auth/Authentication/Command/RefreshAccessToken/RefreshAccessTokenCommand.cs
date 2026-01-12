using BeerStore.Application.DTOs.Auth.Authentication.Responses.Login;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string RefreshToken, string DeviceId) : IRequest<LoginResponse>;
}