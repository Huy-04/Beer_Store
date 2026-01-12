namespace BeerStore.Application.DTOs.Auth.Authentication.Responses.Login
{
    public record LoginResponse(
        string Token,
        Guid UserId,
        string Email,
        string UserName,
        List<string> Roles,
        string RefreshToken,
        DateTimeOffset ExpiresAt)
    {
    }
}