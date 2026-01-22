using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Queries.GetUserPermissionsByUserId
{
    public record GetUserPermissionsByUserIdQuery(Guid UserId) : IRequest<IEnumerable<UserPermissionResponse>>;
}
