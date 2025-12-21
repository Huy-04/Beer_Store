using MediatR;

namespace BeerStore.Application.Modules.Auth.User.Commands.RemoveUser
{
    public record RemoveUserCommand(Guid IdUser) : IRequest<bool>
    {
    }
}