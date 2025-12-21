using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Domain.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permission.Queries.GetPermissionByModule
{
    public record GetPermissionByModuleQuery(ModuleEnum Module) : IRequest<IEnumerable<PermissionResponse>>
    {
    }
}