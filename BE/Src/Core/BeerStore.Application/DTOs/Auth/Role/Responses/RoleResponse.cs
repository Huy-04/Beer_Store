namespace BeerStore.Application.DTOs.Auth.Role.Responses
{
    public record RoleResponse(
        Guid IdRole,
        string RoleName,
        string Description,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}