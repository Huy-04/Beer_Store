using BeerStore.Application.DTOs.Auth.Role.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Roles.Queries.GetRoleById
{
    public record GetRoleByIdQuery(Guid IdRole) : IRequest<RoleResponse>
    {
    }
}
