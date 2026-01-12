namespace BeerStore.Application.DTOs.Auth.Authentication.Responses.Register
{
    public record RegisterResponse(
        Guid Id,
        string Email,
        string UserName,
        string FullName,
        DateTimeOffset CreatedAt);
}
