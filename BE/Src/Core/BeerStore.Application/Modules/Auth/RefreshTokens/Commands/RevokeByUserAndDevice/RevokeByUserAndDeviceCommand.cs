using MediatR;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeByUserAndDevice
{
    public record RevokeByUserAndDeviceCommand(Guid UpdatedBy, Guid UserId, string DeviceId) : IRequest;
}