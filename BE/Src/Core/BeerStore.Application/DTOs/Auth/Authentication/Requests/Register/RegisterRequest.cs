namespace BeerStore.Application.DTOs.Auth.Authentication.Requests.Register
{
    public record RegisterRequest(
        string Email,
        string Phone,
        string FullName,
        string UserName,
        string Password)
    {
    }
}
