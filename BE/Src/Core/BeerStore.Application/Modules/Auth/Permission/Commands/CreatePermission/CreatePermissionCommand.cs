using BeerStore.Application.DTOs.Auth.Permission.Requests;
using BeerStore.Application.DTOs.Auth.Permission.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permission.Commands.CreatePermission
{
    public record CreatePermissionCommand(Guid CreatedBy, Guid UpdateBy, PermissionRequest Request) : IRequest<PermissionResponse>
    {
    }
}