using MediatR;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.Logout
{
    public record LogoutCommand(Guid UserId, string DeviceId) : IRequest;
}