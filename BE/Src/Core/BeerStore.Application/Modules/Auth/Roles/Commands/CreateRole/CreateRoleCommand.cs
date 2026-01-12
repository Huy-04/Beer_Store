using BeerStore.Application.DTOs.Auth.Role.Requests;
using BeerStore.Application.DTOs.Auth.Role.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Roles.Commands.CreateRole
{
    public record CreateRoleCommand(Guid CreatedBy, Guid UpdatedBy, RoleRequest Request) : IRequest<RoleResponse>
    {
    }
}
