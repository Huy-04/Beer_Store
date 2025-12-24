using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetUserRolesByUserId
{
    public record GetUserRolesByUserIdQuery(Guid UserId) : IRequest<IEnumerable<UserRoleResponse>>;
}
