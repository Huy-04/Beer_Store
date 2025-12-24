using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;

namespace BeerStore.Application.Mapping.Auth.Junction.RolePermissionMap
{
    public static class RolePermissionToResponse
    {
        public static RolePermissionResponse ToRolePermissionResponse(this RolePermission rolePermission)
        {
            return new RolePermissionResponse(rolePermission.Id, rolePermission.RoleId, rolePermission.PermissionId);
        }
    }
}
