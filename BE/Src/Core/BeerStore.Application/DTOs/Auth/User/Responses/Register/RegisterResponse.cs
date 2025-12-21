namespace BeerStore.Application.DTOs.Auth.User.Responses.Register
{
    public record RegisterResponse(
        Guid Id,
        string Email,
        string UserName,
        string FullName,
        DateTimeOffset CreatedAt,
        string Message)
    {
    }
}