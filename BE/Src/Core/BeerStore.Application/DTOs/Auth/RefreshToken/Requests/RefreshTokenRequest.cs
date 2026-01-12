using BeerStore.Domain.ValueObjects.Auth.RefreshToken;

namespace BeerStore.Application.DTOs.Auth.RefreshToken.Requests
{
    public record RefreshTokenRequest(
        Guid userId,
        DeviceId deviceId,
        DeviceName deviceName,
        IpAddress ipAddress)
    {
    }
}