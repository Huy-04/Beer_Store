using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Commands.RemoveUser
{
    public record RemoveUserCommand(Guid IdUser) : IRequest<bool>
    {
    }
}
