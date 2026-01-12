namespace BeerStore.Application.DTOs.Auth.Authentication.Requests.Login
{
    public record LoginRequest(
        string Email,
        string Password);
}
