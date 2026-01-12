using BeerStore.Application.DTOs.Auth.Permission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetAllPermission
{
    public record GetAllPermissionQuery() : IRequest<IEnumerable<PermissionResponse>>
    {
    }
}
