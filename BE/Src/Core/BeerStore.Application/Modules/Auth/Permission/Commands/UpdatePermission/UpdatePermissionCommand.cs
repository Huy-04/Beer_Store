using BeerStore.Application.DTOs.Auth.Permission.Requests;
using BeerStore.Application.DTOs.Auth.Permission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permission.Commands.UpdatePermission
{
    public record UpdatePermissionCommand(Guid IdPermission, Guid UpdatedBy, PermissionRequest Request) : IRequest<PermissionResponse>
    {
    }
}