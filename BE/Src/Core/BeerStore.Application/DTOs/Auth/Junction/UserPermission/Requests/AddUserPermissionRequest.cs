using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.Junction.UserPermission.Requests
{
    public record AddUserPermissionRequest(Guid UserId, Guid PermissionId, StatusEnum Status);
}
