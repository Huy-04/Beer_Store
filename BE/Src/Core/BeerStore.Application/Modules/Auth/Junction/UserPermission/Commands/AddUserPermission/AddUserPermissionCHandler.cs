using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserPermissionMap;
using UserPermissionEntity = BeerStore.Domain.Entities.Auth.Junction.UserPermission;
using BeerStore.Domain.Enums.Messages.Junction;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Commands.AddUserPermission
{
    public class AddUserPermissionCHandler : IRequestHandler<AddUserPermissionCommand, UserPermissionResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<AddUserPermissionCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public AddUserPermissionCHandler(IAuthUnitOfWork auow, ILogger<AddUserPermissionCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserPermissionResponse> Handle(AddUserPermissionCommand command, CancellationToken token)
        {
            _authService.EnsureCanAddUserPermission();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = await _auow.RUserRepository.GetByIdAsync(command.UserId, token);
                if (user == null)
                    throw new BusinessRuleException<UserPermissionField>(
                        ErrorCategory.NotFound,
                        UserPermissionField.UserId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.UserId }
                        });

                var permission = await _auow.RPermissionRepository.GetByIdAsync(command.PermissionId, token);
                if (permission == null)
                    throw new BusinessRuleException<UserPermissionField>(
                        ErrorCategory.NotFound,
                        UserPermissionField.PermissionId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.PermissionId }
                        });

                // Check duplicate
                if (await _auow.RUserPermissionRepository.ExistsAsync(command.UserId, command.PermissionId, token))
                    throw new BusinessRuleException<UserPermissionField>(
                        ErrorCategory.Conflict,
                        UserPermissionField.PermissionId,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.PermissionId }
                        });

                var userPermission = UserPermissionEntity.Create(command.UserId, command.PermissionId, command.Status);

                await _auow.WUserPermissionRepository.AddAsync(userPermission, token);
                await _auow.CommitTransactionAsync(token);

                return userPermission.ToUserPermissionResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to add UserPermission. UserId: {UserId}, PermissionId: {PermissionId}",
                    command.UserId, command.PermissionId
                );
                throw;
            }
        }
    }
}