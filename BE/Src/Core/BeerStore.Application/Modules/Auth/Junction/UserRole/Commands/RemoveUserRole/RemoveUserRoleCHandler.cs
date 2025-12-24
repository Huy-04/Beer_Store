using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Commands.RemoveUserRole
{
    public class RemoveUserRoleCHandler : IRequestHandler<RemoveUserRoleCommand, Unit>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserRoleCHandler> _logger;

        public RemoveUserRoleCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserRoleCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveUserRoleCommand command, CancellationToken token)
        {
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

                _auow.WUserRoleRepository.Remove(userRole);
                await _auow.CommitTransactionAsync(token);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while removing UserRole. UserRoleId: {UserRoleId}",
                    command.UserRoleId
                );
                throw;
            }
        }
    }
}
