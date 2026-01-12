using BeerStore.Application.DTOs.Auth.Permission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetPermissionById
{
    public record GetPermissionByIdQuery(Guid IdPermission) : IRequest<PermissionResponse>
    {
    }
}
