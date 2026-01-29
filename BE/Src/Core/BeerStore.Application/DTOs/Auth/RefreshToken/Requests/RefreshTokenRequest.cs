using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using Domain.Core.ValueObjects.Common;

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