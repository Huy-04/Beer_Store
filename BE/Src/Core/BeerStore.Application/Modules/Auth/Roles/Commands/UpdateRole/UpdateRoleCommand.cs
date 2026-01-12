using BeerStore.Application.DTOs.Auth.Role.Requests;
using BeerStore.Application.DTOs.Auth.Role.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Roles.Commands.UpdateRole
{
    public record UpdateRoleCommand(Guid IdRole, Guid UpdatedBy, RoleRequest Request) : IRequest<RoleResponse>
    {
    }
}
