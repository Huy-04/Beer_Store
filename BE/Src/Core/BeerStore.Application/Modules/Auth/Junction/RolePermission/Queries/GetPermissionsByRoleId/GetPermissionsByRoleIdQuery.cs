using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Queries.GetPermissionsByRoleId
{
    public record GetPermissionsByRoleIdQuery(Guid RoleId) : IRequest<IEnumerable<RolePermissionResponse>>;
}
