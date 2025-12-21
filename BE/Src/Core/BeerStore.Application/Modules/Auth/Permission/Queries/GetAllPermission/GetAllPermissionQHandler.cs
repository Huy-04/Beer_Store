using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Permission.Queries.GetAllPermission
{
    public class GetAllPermissionQHandler : IRequestHandler<GetAllPermissionQuery, IEnumerable<PermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetAllPermissionQHandler> _logger;

        public GetAllPermissionQHandler(IAuthUnitOfWork auow, ILogger<GetAllPermissionQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionResponse>> Handle(GetAllPermissionQuery query, CancellationToken token)
        {
            var list = await _auow.RPermissionRepository.GetAllAsync(token);
            return list.Select(p => p.ToPermissionResponse());
        }
    }
}