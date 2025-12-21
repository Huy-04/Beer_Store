using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RoleMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Role.Queries.GetRoleById
{
    public class GetRoleByIdQHandler : IRequestHandler<GetRoleByIdQuery, RoleResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetRoleByIdQHandler> _logger;

        public GetRoleByIdQHandler(IAuthUnitOfWork auow, ILogger<GetRoleByIdQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<RoleResponse> Handle(GetRoleByIdQuery query, CancellationToken token)
        {
            var role = await _auow.RRoleRepository.GetByIdAsync(query.IdRole);
            if (role == null)
            {
                _logger.LogWarning("Get failed: Role with Id={Id} not found", query.IdRole);
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
