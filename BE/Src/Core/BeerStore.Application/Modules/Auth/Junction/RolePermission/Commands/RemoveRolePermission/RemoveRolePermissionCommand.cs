using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Commands.RemoveRolePermission
{
    public record RemoveRolePermissionCommand(Guid RolePermissionId) : IRequest<Unit>;
}
