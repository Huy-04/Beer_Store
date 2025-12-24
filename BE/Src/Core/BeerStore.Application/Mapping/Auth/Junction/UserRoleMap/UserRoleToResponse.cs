using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;

namespace BeerStore.Application.Mapping.Auth.Junction.UserRoleMap
{
    public static class UserRoleToResponse
    {
        public static UserRoleResponse ToUserRoleResponse(this UserRole userRole)
        {
            return new UserRoleResponse(userRole.Id, userRole.UserId, userRole.RoleId);
        }
    }
}
