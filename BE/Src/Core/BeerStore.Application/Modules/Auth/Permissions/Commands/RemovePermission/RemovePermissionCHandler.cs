using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Permissions.Commands.RemovePermission
{
    public class RemovePermissionCHandler : IRequestHandler<RemovePermissionCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemovePermissionCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemovePermissionCHandler(IAuthUnitOfWork auow, ILogger<RemovePermissionCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<bool> Handle(RemovePermissionCommand command, CancellationToken token)
        {
            _authService.EnsureCanRemovePermission();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var permission = await _auow.RPermissionRepository.GetByIdAsync(command.IdPermission, token);
                if (permission == null)
                {
                    _logger.LogWarning("Permission {Id} not found", command.IdPermission);
                    throw new BusinessRuleException<PermissionField>(
                        ErrorCategory.NotFound,
                        PermissionField.IdPermission,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value,command.IdPermission }
                        });
                }

                _auow.WPermissionRepository.Remove(permission);
                await _auow.CommitTransactionAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove Permission. Id: {Id}",
                    command.IdPermission
                );
                throw;
            }
        }
    }
}

