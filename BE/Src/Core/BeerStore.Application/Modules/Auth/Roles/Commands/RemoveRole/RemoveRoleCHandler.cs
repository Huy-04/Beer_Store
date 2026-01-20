using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Roles.Commands.RemoveRole
{
    public class RemoveRoleCHandler : IRequestHandler<RemoveRoleCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveRoleCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveRoleCHandler(IAuthUnitOfWork auow, ILogger<RemoveRoleCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<bool> Handle(RemoveRoleCommand command, CancellationToken token)
        {
            _authService.EnsureCanRemoveRole();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var role = await _auow.RRoleRepository.GetByIdAsync(command.IdRole, token);
                if (role == null)
                {
                    _logger.LogWarning("Role {Id} not found", command.IdRole);
                    throw new BusinessRuleException<RoleField>(
                        ErrorCategory.NotFound,
                        RoleField.IdRole,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value, command.IdRole }
                        });
                }

                _auow.WRoleRepository.Remove(role);
                await _auow.CommitTransactionAsync(token);

                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove Role. Id: {Id}",
                    command.IdRole
                );
                throw;
            }
        }
    }
}

