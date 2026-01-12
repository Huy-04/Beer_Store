using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCHandler : IRequestHandler<CreatePermissionCommand, PermissionResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<CreatePermissionCHandler> _logger;

        public CreatePermissionCHandler(IAuthUnitOfWork auow, ILogger<CreatePermissionCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<PermissionResponse> Handle(CreatePermissionCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var permission = command.Request.ToPermission(command.CreatedBy, command.UpdateBy);

                if (await _auow.RPermissionRepository.ExistsByNameAsync(permission.PermissionName, token))
                {
                    _logger.LogWarning("Permission with Name {Name} already exists", permission.PermissionName.Value);
                    throw new BusinessRuleException<PermissionField>(
                        ErrorCategory.Conflict,
                        PermissionField.PermissionName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value, permission.PermissionName.Value } }
                        );
                }

                await _auow.WPermissionRepository.AddAsync(permission, token);
                await _auow.CommitTransactionAsync(token);

                return permission.ToPermissionResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to create Permission. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
