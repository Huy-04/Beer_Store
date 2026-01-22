using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.RemoveUserRole
{
    public record RemoveUserRoleCommand(Guid UserRoleId) : IRequest;
}
