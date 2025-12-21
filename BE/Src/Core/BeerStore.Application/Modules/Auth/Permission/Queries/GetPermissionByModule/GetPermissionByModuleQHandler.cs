using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using BeerStore.Domain.ValueObjects.Auth.Permissions.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Permission.Queries.GetPermissionByModule
{
    public class GetPermissionByModuleQHandler : IRequestHandler<GetPermissionByModuleQuery, IEnumerable<PermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetPermissionByModuleQHandler> _logger;

        public GetPermissionByModuleQHandler(IAuthUnitOfWork auow, ILogger<GetPermissionByModuleQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionResponse>> Handle(GetPermissionByModuleQuery query, CancellationToken token)
        {
            var module = Module.Create(query.Module);
            var list = await _auow.RPermissionRepository.FindAsync(p => p.Module == module, token);
            return list.Select(p => p.ToPermissionResponse());
        }
    }
}