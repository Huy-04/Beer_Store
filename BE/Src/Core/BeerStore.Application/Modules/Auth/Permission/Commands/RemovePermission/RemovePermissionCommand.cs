using MediatR;

namespace BeerStore.Application.Modules.Auth.Permission.Commands.RemovePermission
{
    public record RemovePermissionCommand(Guid IdPermission) : IRequest<bool>
    {
    }
}