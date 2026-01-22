using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using Domain.Core.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.AddUserPermission
{
    public record AddUserPermissionCommand(Guid UserId, Guid PermissionId, StatusEnum Status) : IRequest<UserPermissionResponse>;
}
