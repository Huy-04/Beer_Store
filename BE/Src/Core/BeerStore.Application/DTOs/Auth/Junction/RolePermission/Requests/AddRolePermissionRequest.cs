namespace BeerStore.Application.DTOs.Auth.Junction.RolePermission.Requests
{
    public record AddRolePermissionRequest(Guid RoleId, Guid PermissionId);
}
