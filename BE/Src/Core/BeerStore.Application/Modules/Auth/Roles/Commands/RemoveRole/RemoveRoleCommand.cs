using BeerStore.Application.DTOs.Auth.Role.Requests;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Roles.Commands.RemoveRole
{
    public record RemoveRoleCommand(Guid IdRole) : IRequest<bool>
    {
    }
}
