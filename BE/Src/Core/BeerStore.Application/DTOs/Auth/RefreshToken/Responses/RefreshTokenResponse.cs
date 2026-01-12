namespace BeerStore.Application.DTOs.Auth.RefreshToken.Responses
{
    public record RefreshTokenResponse(
        string refreshToken,
        DateTimeOffset expiresAt);
}