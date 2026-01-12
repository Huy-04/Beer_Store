using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.RefreshTokenMap
{
    public static class ToSessionResponse
    {
        public static SessionResponse ToSession(this RefreshToken refreshToken)
        {
            return new SessionResponse(
                refreshToken.Id,
                refreshToken.DeviceId,
                refreshToken.DeviceName,
                refreshToken.IpAddress,
                refreshToken.CreatedAt,
                refreshToken.ExpiresAt,
                refreshToken.TokenStatus.Value);
        }
    }
}