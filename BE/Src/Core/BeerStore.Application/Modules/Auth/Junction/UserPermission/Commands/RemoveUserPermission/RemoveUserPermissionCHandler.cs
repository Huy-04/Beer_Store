using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages.Junction;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.RemoveUserPermission
{
    public class RemoveUserPermissionCHandler : IRequestHandler<RemoveUserPermissionCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserPermissionCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveUserPermissionCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserPermissionCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task Handle(RemoveUserPermissionCommand command, CancellationToken token)
        {
            _authService.EnsureCanRemoveUserPermission();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var userPermission = await _auow.RUserPermissionRepository.GetByIdAsync(command.Id, token);
                if (userPermission == null)
                    throw new BusinessRuleException<UserPermissionField>(
                        ErrorCategory.NotFound,
                        UserPermissionField.UserId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.Id }
                        });

                _auow.WUserPermissionRepository.Remove(userPermission);
                await _auow.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove UserPermission. Id: {Id}",
                    command.Id
                );
                throw;
            }
        }
    }
}
