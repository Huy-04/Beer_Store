using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.RolePermissionMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Queries.GetPermissionsByRoleId
{
    public class GetPermissionsByRoleIdQHandler : IRequestHandler<GetPermissionsByRoleIdQuery, IEnumerable<RolePermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetPermissionsByRoleIdQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<RolePermissionResponse>> Handle(GetPermissionsByRoleIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadRolePermission();

            var rolePermissions = await _auow.RRolePermissionRepository.FindAsync(rp => rp.RoleId == query.RoleId, token);
            return rolePermissions.Select(rp => rp.ToRolePermissionResponse());
        }
    }
}

