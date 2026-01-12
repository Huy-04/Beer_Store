using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQHandler : IRequestHandler<GetPermissionByIdQuery, PermissionResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetPermissionByIdQHandler> _logger;

        public GetPermissionByIdQHandler(IAuthUnitOfWork auow, ILogger<GetPermissionByIdQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<PermissionResponse> Handle(GetPermissionByIdQuery query, CancellationToken token)
        {
            var permission = await _auow.RPermissionRepository.GetByIdAsync(query.IdPermission, token);

            if (permission == null)
            {
                _logger.LogWarning("Permission {Id} not found", query.IdPermission);
                throw new BusinessRuleException<PermissionField>(
                    ErrorCategory.NotFound,
                    PermissionField.IdPermission,
                    ErrorCode.IdNotFound,
                    new Dictionary<object, object>
                    {
                            {ParamField.Value,query.IdPermission }
                    });
            }

            return permission.ToPermissionResponse();
        }
    }
}
