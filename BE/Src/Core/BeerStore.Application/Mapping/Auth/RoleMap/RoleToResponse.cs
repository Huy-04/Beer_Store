using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.RoleMap
{
    public static class RoleToResponse
    {
        public static RoleResponse ToRoleResponse(this Role role)
        {
            return new(
                role.Id,
                role.RoleName.Value,
                role.Description.Value,
                role.CreatedBy,
                role.UpdatedBy,
                role.CreatedAt,
                role.UpdatedAt);
        }
    }
}
