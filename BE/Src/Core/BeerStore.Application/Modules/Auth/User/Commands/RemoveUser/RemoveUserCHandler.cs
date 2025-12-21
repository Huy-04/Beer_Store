using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Commands.RemoveUser
{
    public class RemoveUserCHandler : IRequestHandler<RemoveUserCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserCHandler> _logger;

        public RemoveUserCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveUserCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = await _auow.RUserRepository.GetByIdAsync(command.IdUser, token);

                if (user == null)
                {
                    _logger.LogWarning("Remove failed: User with Id={Id} not found", command.IdUser);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.NotFound,
                        UserField.IdUser,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value, command.IdUser },
                        });
                }

                _auow.WUserRepository.Remove(user);
                await _auow.CommitTransactionAsync(token);

                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while removing User. Id: {Id}",
                    command.IdUser
                );
                throw;
            }
        }
    }
}