using BeerStore.Application.DTOs.Auth.Role.Requests;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Roles;
using Domain.Core.ValueObjects;

namespace BeerStore.Application.Mapping.Auth.RoleMap
{
    public static class RequestToRole
    {
        public static Role ToRole(this RoleRequest request, Guid createdBy, Guid updatedBy)
        {
            return Role.Create(
                RoleName.Create(request.RoleName),
                Description.Create(request.Description),
                createdBy,
                updatedBy);
        }

        public static void ApplyRole(this Role role, RoleRequest request, Guid updatedBy)
        {
            role.updateRoleName(RoleName.Create(request.RoleName));
            role.updateDescription(Description.Create(request.Description));
            role.SetUpdatedBy(updatedBy);
        }
    }
}