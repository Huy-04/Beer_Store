using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetAllUserRoles
{
    public record GetAllUserRolesQuery : IRequest<IEnumerable<UserRoleResponse>>;
}
