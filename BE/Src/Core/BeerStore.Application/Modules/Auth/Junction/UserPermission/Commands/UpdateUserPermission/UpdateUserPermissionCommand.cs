using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using Domain.Core.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.UpdateUserPermission
{
    public record UpdateUserPermissionCommand(Guid Id, StatusEnum Status) : IRequest<UserPermissionResponse>;
}
