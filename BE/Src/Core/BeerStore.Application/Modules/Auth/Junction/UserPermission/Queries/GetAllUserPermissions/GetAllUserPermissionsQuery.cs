using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Queries.GetAllUserPermissions
{
    public record GetAllUserPermissionsQuery : IRequest<IEnumerable<UserPermissionResponse>>;
}
