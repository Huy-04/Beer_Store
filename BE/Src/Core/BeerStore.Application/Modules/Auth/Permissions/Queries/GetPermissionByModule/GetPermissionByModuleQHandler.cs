using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using BeerStore.Domain.ValueObjects.Auth.Permission.Enums;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetPermissionByModule
{
    public class GetPermissionByModuleQHandler : IRequestHandler<GetPermissionByModuleQuery, IEnumerable<PermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetPermissionByModuleQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<PermissionResponse>> Handle(GetPermissionByModuleQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadPermission();

            var module = Module.Create(query.Module);
            var list = await _auow.RPermissionRepository.FindAsync(p => p.Module == module, token);
            return list.Select(p => p.ToPermissionResponse());
        }
    }
}

