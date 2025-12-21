namespace BeerStore.Application.DTOs.Auth.User.Responses.Login
{
    public record LoginResponse(
        string Token,
        Guid UserId,
        string Email,
        string UserName,
        List<string> Roles)
    {
    }
}