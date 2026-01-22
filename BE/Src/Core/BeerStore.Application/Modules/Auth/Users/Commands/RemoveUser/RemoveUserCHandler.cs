using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Users.Commands.RemoveUser
{
    public class RemoveUserCHandler : IRequestHandler<RemoveUserCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveUserCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task Handle(RemoveUserCommand command, CancellationToken token)
        {
            _authService.EnsureCanRemoveUser();

            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = await _auow.RUserRepository.GetByIdAsync(command.IdUser, token);

                if (user == null)
                {
                    _logger.LogWarning("User {Id} not found", command.IdUser);
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
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to removing User. Id: {Id}",
                    command.IdUser
                );
                throw;
            }
        }
    }
}

