using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserPermissionMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Queries.GetAllUserPermissions
{
    public class GetAllUserPermissionsQHandler : IRequestHandler<GetAllUserPermissionsQuery, IEnumerable<UserPermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllUserPermissionsQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserPermissionResponse>> Handle(GetAllUserPermissionsQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadUserPermission();

            var list = await _auow.RUserPermissionRepository.GetAllAsync(token);
            return list.Select(up => up.ToUserPermissionResponse());
        }
    }
}
