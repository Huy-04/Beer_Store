using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetAllPermission
{
    public class GetAllPermissionQHandler : IRequestHandler<GetAllPermissionQuery, IEnumerable<PermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllPermissionQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<PermissionResponse>> Handle(GetAllPermissionQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadPermission();

            var list = await _auow.RPermissionRepository.GetAllAsync(token);
            return list.Select(p => p.ToPermissionResponse());
        }
    }
}

