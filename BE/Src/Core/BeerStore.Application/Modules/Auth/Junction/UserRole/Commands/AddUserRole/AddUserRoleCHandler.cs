using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserRoleMap;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.Enums.Messages;
using UserRoleEntity = BeerStore.Domain.Entities.Auth.Junction.UserRole;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.AddUserRole
{
    public class AddUserRoleCHandler : IRequestHandler<AddUserRoleCommand, UserRoleResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<AddUserRoleCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public AddUserRoleCHandler(IAuthUnitOfWork auow, ILogger<AddUserRoleCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserRoleResponse> Handle(AddUserRoleCommand command, CancellationToken token)
        {
            _authService.EnsureCanAddUserRole();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = await _auow.RUserRepository.GetByIdAsync(command.UserId, token);
                if (user == null)
                    throw new BusinessRuleException<UserRoleField>(
                        ErrorCategory.NotFound,
                        UserRoleField.UserId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.UserId }
                        });

                var role = await _auow.RRoleRepository.GetByIdAsync(command.RoleId, token);
                if (role == null)
                    throw new BusinessRuleException<UserRoleField>(
                        ErrorCategory.NotFound,
                        UserRoleField.RoleId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.RoleId }
                        });

                var userRole = UserRoleEntity.Create(command.UserId, command.RoleId);

                await _auow.WUserRoleRepository.AddAsync(userRole, token);
                await _auow.CommitTransactionAsync(token);

                return userRole.ToUserRoleResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to add UserRole. UserId: {UserId}, RoleId: {RoleId}",
                    command.UserId, command.RoleId
                );
                throw;
            }
        }
    }
}

