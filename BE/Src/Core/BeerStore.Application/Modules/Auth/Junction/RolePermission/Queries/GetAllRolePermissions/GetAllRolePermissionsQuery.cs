using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Queries.GetAllRolePermissions
{
    public record GetAllRolePermissionsQuery : IRequest<IEnumerable<RolePermissionResponse>>;
}
