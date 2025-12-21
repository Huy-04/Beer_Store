namespace BeerStore.Application.DTOs.Auth.User.Requests
{
    public record CreateUserRequest(
        string Email,
        string Phone,
        string FullName,
        string UserName,
        string Password)
    {
    }
}