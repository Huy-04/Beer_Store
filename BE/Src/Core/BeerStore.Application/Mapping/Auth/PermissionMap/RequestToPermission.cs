using BeerStore.Application.DTOs.Auth.Permission.Requests;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Permission;
using BeerStore.Domain.ValueObjects.Auth.Permission.Enums;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Application.Mapping.Auth.PermissionMap
{
    public static class RequestToPermission
    {
        public static Permission ToPermission(this PermissionRequest request, Guid createdBy, Guid updatedBy)
        {
            return Permission.Create(
                PermissionName.Create(request.PermissionName),
                Module.Create(request.Module),
                Operation.Create(request.Operation),
                Description.Create(request.Description),
                createdBy,
                updatedBy);
        }

        public static void ApplyPermission(this Permission permission, Guid updatedBy, PermissionRequest request)
        {
            permission.UpdatePermissionName(PermissionName.Create(request.PermissionName));
            permission.UpdateModule(Module.Create(request.Module));
            permission.UpdateOperation(Operation.Create(request.Operation));
            permission.UpdateDescription(Description.Create(request.Description));
            permission.SetUpdatedBy(updatedBy);
        }
    }
}
