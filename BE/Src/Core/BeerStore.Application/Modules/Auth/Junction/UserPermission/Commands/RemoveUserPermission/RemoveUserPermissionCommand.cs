using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.RemoveUserPermission
{
    public record RemoveUserPermissionCommand(Guid Id) : IRequest;
}
