using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RoleMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQHandler : IRequestHandler<GetRoleByIdQuery, RoleResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetRoleByIdQHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public GetRoleByIdQHandler(IAuthUnitOfWork auow, ILogger<GetRoleByIdQHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<RoleResponse> Handle(GetRoleByIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadRole();

            var role = await _auow.RRoleRepository.GetByIdAsync(query.IdRole);
            if (role == null)
            {
                _logger.LogWarning("Role {Id} not found", query.IdRole);
                throw new BusinessRuleException<RoleField>(
                    ErrorCategory.NotFound,
                    RoleField.IdRole,
                    ErrorCode.IdNotFound,
                    new Dictionary<object, object>
                    {
                            {ParamField.Value,query.IdRole }
                    });
            }
            return role.ToRoleResponse();
        }
    }
}

