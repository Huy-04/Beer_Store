using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Commands.AddRolePermission
{
    public record AddRolePermissionCommand(Guid RoleId, Guid PermissionId) : IRequest<RolePermissionResponse>;
}
