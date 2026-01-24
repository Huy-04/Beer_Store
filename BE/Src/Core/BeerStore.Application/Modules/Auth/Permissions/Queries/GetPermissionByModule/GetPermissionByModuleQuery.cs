using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Domain.Enums.Auth;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetPermissionByModule
{
    public record GetPermissionByModuleQuery(ModuleEnum Module) : IRequest<IEnumerable<PermissionResponse>>
    {
    }
}
