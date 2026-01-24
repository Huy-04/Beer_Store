using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Auth.Messages.Junction;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.RolePermission.Commands.RemoveRolePermission
{
    public class RemoveRolePermissionCHandler : IRequestHandler<RemoveRolePermissionCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveRolePermissionCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveRolePermissionCHandler(IAuthUnitOfWork auow, ILogger<RemoveRolePermissionCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task Handle(RemoveRolePermissionCommand command, CancellationToken token)
        {
            _authService.EnsureCanRemoveRolePermission();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var rolePermission = await _auow.RRolePermissionRepository.GetByIdAsync(command.RolePermissionId, token);

                if (rolePermission == null)
                    throw new BusinessRuleException<RolePermissionField>(
                        ErrorCategory.NotFound,
                        RolePermissionField.RoleId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.RolePermissionId }
                        });

                _auow.WRolePermissionRepository.Remove(rolePermission);
                await _auow.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove RolePermission. RolePermissionId: {RolePermissionId}",
                    command.RolePermissionId
                );
                throw;
            }
        }
    }
}

