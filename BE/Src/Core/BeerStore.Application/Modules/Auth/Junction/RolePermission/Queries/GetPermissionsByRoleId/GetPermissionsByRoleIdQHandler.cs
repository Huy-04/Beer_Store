using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.JunctionMap.RolePermissionMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Queries.GetPermissionsByRoleId
{
    public class GetPermissionsByRoleIdQHandler : IRequestHandler<GetPermissionsByRoleIdQuery, IEnumerable<RolePermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetPermissionsByRoleIdQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<RolePermissionResponse>> Handle(GetPermissionsByRoleIdQuery query, CancellationToken token)
        {
            var rolePermissions = await _auow.RRolePermissionRepository.FindAsync(rp => rp.RoleId == query.RoleId, token);
            return rolePermissions.Select(rp => rp.ToRolePermissionResponse());
        }
    }
}
