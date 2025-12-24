using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.AddUserRole
{
    public record AddUserRoleCommand(Guid UserId, Guid RoleId) : IRequest<UserRoleResponse>;
}
