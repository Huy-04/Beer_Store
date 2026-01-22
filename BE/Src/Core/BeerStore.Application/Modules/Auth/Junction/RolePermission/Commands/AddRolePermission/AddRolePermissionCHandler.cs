using BeerStore.Application.DTOs.Auth.Junction.RolePermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.RolePermissionMap;
using RolePermissionEntity = BeerStore.Domain.Entities.Auth.Junction.RolePermission;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;
using BeerStore.Domain.Enums.Messages.Junction;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Commands.AddRolePermission
{
    public class AddRolePermissionCHandler : IRequestHandler<AddRolePermissionCommand, RolePermissionResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<AddRolePermissionCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public AddRolePermissionCHandler(IAuthUnitOfWork auow, ILogger<AddRolePermissionCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<RolePermissionResponse> Handle(AddRolePermissionCommand command, CancellationToken token)
        {
            _authService.EnsureCanAddRolePermission();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var role = await _auow.RRoleRepository.GetByIdAsync(command.RoleId, token);
                if (role == null)
                    throw new BusinessRuleException<RolePermissionField>(
                        ErrorCategory.NotFound,
                        RolePermissionField.RoleId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.RoleId }
                        });

                var permission = await _auow.RPermissionRepository.GetByIdAsync(command.PermissionId, token);
                if (permission == null)
                    throw new BusinessRuleException<RolePermissionField>(
                        ErrorCategory.NotFound,
                        RolePermissionField.PermissionId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.PermissionId }
                        });

                // Check duplicate
                if (await _auow.RRolePermissionRepository.ExistsAsync(command.RoleId, command.PermissionId, token))
                    throw new BusinessRuleException<RolePermissionField>(
                        ErrorCategory.Conflict,
                        RolePermissionField.PermissionId,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.PermissionId }
                        });

                var rolePermission = RolePermissionEntity.Create(command.RoleId, command.PermissionId);

                await _auow.WRolePermissionRepository.AddAsync(rolePermission, token);
                await _auow.CommitTransactionAsync(token);

                return rolePermission.ToRolePermissionResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to add RolePermission. RoleId: {RoleId}, PermissionId: {PermissionId}",
                    command.RoleId, command.PermissionId
                );
                throw;
            }
        }
    }
}