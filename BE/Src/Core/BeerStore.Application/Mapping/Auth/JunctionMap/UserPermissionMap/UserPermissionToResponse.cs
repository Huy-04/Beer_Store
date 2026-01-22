using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using UP = BeerStore.Domain.Entities.Auth.Junction.UserPermission;

namespace BeerStore.Application.Mapping.Auth.JunctionMap.UserPermissionMap
{
    public static class UserPermissionToResponse
    {
        public static UserPermissionResponse ToUserPermissionResponse(this UP userPermission)
        {
            return new UserPermissionResponse(
                userPermission.Id,
                userPermission.UserId,
                userPermission.PermissionId,
                userPermission.Status);
        }
    }
}
