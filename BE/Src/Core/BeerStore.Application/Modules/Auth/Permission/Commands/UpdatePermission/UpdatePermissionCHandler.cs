using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using BeerStore.Domain.Enums.Messages;
using BeerStore.Domain.ValueObjects.Auth.Permissions;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Permission.Commands.UpdatePermission
{
    public class UpdatePermissionCHandler : IRequestHandler<UpdatePermissionCommand, PermissionResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdatePermissionCHandler> _logger;

        public UpdatePermissionCHandler(IAuthUnitOfWork auow, ILogger<UpdatePermissionCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<PermissionResponse> Handle(UpdatePermissionCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var permission = await _auow.RPermissionRepository.GetByIdAsync(command.IdPermission, token);
                if (permission == null)
                {
                    _logger.LogWarning("Update failed: Permission with Id={Id} not found", command.IdPermission);
                    throw new BusinessRuleException<PermissionField>(
                        ErrorCategory.NotFound,
                        PermissionField.IdPermission,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value.ToString(),command.IdPermission }
                        });
                }

                var newName = PermissionName.Create(command.Request.PermissionName);

                if (await _auow.RPermissionRepository.ExistsByNameAsync(newName, token, permission.Id))
                {
                    _logger.LogWarning("Update failed: Permission with Name '{Name}' already exists", newName.Value);
                    throw new BusinessRuleException<PermissionField>(
                        ErrorCategory.Conflict,
                        PermissionField.PermissionName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value.ToString(), newName.Value }
                        });
                }

                permission.ApplyPermission(command.UpdatedBy, command.Request);
                _auow.WPermissionRepository.Update(permission);
                await _auow.CommitTransactionAsync(token);

                return permission.ToPermissionResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while updating Permission. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
