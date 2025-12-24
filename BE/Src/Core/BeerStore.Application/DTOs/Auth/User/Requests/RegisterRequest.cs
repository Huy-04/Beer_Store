namespace BeerStore.Application.DTOs.Auth.User.Requests
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
