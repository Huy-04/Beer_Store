using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.RefreshTokenMap
{
    public static class RefreshTokenToResponse
    {
        public static RefreshTokenResponse ToRefreshTokenResponse(this RefreshToken refreshToken, string rawToken)
        {
            return new(rawToken, refreshToken.ExpiresAt);
        }
    }
}