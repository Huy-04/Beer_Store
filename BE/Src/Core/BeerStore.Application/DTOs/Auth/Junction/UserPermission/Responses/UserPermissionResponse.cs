using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses
{
    public record UserPermissionResponse(Guid Id, Guid UserId, Guid PermissionId, StatusEnum Status);
}
