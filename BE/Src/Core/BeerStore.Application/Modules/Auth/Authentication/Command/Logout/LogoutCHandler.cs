using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using BeerStore.Domain.Enums.Auth.Messages;
using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.Logout
{
    public class LogoutCHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<LogoutCHandler> _logger;

        public LogoutCHandler(IAuthUnitOfWork auow, ILogger<LogoutCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task Handle(LogoutCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var userSystem = await _auow.RUserRepository.GetByUserNameAsync(UserName.System);
                if (userSystem == null)
                {
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.NotFound,
                        UserField.UserName,
                        ErrorCode.UserNameNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value, UserName.System.Value }
                        });
                }

                var existingToken = await _auow.RRefreshTokenRepository.
                    GetByUserIdAndDeviceIdAsync(command.UserId, command.DeviceId, token);

                if (existingToken == null)
                {
                    throw new BusinessRuleException<RefreshTokenField>(
                        ErrorCategory.NotFound,
                        RefreshTokenField.TokenHash,
                        ErrorCode.TokenNotFound,
                        new Dictionary<object, object>());
                }

                existingToken.ApplyRefreshToke(userSystem.Id);
                _auow.WRefreshTokenRepository.Update(existingToken);

                await _auow.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to logout. UserId: {UserId}", command.UserId);
                throw;
            }
        }
    }
}