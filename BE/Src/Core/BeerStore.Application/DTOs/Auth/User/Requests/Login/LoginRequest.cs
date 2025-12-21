namespace BeerStore.Application.DTOs.Auth.User.Requests.Login
{
    public record LoginRequest(
        string Email,
        string Password)
    {
    }
}