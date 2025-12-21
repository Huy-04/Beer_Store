using BeerStore.Application.DTOs.Auth.Role.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Role.Queries.GetAllRole
{
    public record GetAllRoleQuery : IRequest<IEnumerable<RoleResponse>>
    {
    }
}