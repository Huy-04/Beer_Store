using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserPermissionMap;
using BeerStore.Domain.Enums.Messages.Junction;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.UpdateUserPermission
{
    public class UpdateUserPermissionCHandler : IRequestHandler<UpdateUserPermissionCommand, UserPermissionResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateUserPermissionCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public UpdateUserPermissionCHandler(IAuthUnitOfWork auow, ILogger<UpdateUserPermissionCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserPermissionResponse> Handle(UpdateUserPermissionCommand command, CancellationToken token)
        {
            _authService.EnsureCanUpdateUserPermission();

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

                if (command.Status == StatusEnum.Active)
                    userPermission.Activate();
                else
                    userPermission.Deactivate();

                _auow.WUserPermissionRepository.Update(userPermission);
                await _auow.CommitTransactionAsync(token);

                return userPermission.ToUserPermissionResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to update UserPermission. Id: {Id}",
                    command.Id
                );
                throw;
            }
        }
    }
}
