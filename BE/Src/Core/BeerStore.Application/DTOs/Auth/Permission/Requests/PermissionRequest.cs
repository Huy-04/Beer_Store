using BeerStore.Domain.Enums.Auth;

namespace BeerStore.Application.DTOs.Auth.Permission.Requests
{
    public record PermissionRequest(
        string PermissionName,
        ModuleEnum Module,
        OperationEnum Operation,
        string Description)
    {
    }
}
