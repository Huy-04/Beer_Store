namespace BeerStore.Application.DTOs.Auth.Role.Requests
{
    public record RoleRequest(
        string RoleName,
        string Description)
    {
    }
}
