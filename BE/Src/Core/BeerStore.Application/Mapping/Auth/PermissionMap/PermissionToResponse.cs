using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.PermissionMap
{
    public static class PermissionToResponse
    {
        public static PermissionResponse ToPermissionResponse(this Permission permission)
        {
            return new(
                permission.Id,
                permission.PermissionName.Value,
                permission.Module.Value,
                permission.Operation.Value,
                permission.Description.Value,
                permission.CreatedBy,
                permission.UpdatedBy,
                permission.CreatedAt,
                permission.UpdatedAt);
        }
    }
}