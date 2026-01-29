using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Auth.Messages.Junction;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.RemoveUserRole
{
    public class RemoveUserRoleCHandler : IRequestHandler<RemoveUserRoleCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserRoleCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveUserRoleCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserRoleCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task Handle(RemoveUserRoleCommand command, CancellationToken token)
        {
            _authService.EnsureCanRemoveUserRole();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var userRole = await _auow.RUserRoleRepository.GetByIdAsync(command.UserRoleId, token);

                if (userRole == null)
                    throw new BusinessRuleException<UserRoleField>(
                        ErrorCategory.NotFound,
                        UserRoleField.UserId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.UserRoleId }
                        });

                // Load User
                var user = await _auow.RUserRepository.GetByIdWithRolesAsync(userRole.UserId, token);
                if (user == null) throw new Exception("User not found");

                user.RemoveUserRole(userRole.Id);
                _auow.WUserRepository.Update(user);
                await _auow.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove UserRole. UserRoleId: {UserRoleId}",
                    command.UserRoleId
                );
                throw;
            }
        }
    }
}

