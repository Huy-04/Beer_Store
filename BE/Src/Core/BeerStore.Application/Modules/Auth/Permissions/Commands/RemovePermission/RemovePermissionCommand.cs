using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Commands.RemovePermission
{
    public record RemovePermissionCommand(Guid IdPermission) : IRequest;

}
