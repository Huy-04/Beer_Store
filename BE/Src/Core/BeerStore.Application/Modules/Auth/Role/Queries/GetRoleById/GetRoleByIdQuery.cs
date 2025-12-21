using BeerStore.Application.DTOs.Auth.Role.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Role.Queries.GetRoleById
{
    public record GetRoleByIdQuery(Guid IdRole) : IRequest<RoleResponse>
    {
    }
}