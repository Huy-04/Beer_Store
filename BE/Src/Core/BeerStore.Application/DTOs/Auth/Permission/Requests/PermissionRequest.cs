using BeerStore.Domain.Enums;

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