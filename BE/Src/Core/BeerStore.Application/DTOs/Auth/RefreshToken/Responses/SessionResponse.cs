using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.RefreshToken.Responses
{
    public record SessionResponse(
        Guid Id,
        string DeviceId,
        string DeviceName,
        string IpAddress,
        DateTimeOffset CreatedAt,
        DateTimeOffset ExpiresAt,
        StatusEnum TokenStatus);
}