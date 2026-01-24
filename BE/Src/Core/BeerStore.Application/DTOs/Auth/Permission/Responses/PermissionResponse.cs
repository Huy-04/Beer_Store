using BeerStore.Domain.Enums.Auth;

namespace BeerStore.Application.DTOs.Auth.Permission.Responses
{
    public record PermissionResponse(
        Guid IdPermission,
        string PermissionName,
        ModuleEnum Module,
        OperationEnum Operation,
        string Description,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}
