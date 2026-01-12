using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeRefreshToken
{
    public class RevokeRefreshTokenCHandler : IRequestHandler<RevokeRefreshTokenCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RevokeRefreshTokenCHandler> _logger;

        public RevokeRefreshTokenCHandler(IAuthUnitOfWork auow, ILogger<RevokeRefreshTokenCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<bool> Handle(RevokeRefreshTokenCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var refreshToken = await _auow.RRefreshTokenRepository.FindTokenAsync(token, tokenHash: command.TokenHash);

                if (refreshToken == null)
                {
                    _logger.LogWarning("RefreshToken not found for TokenHash");
                    throw new BusinessRuleException<RefreshTokenField>(
                        ErrorCategory.NotFound,
                        RefreshTokenField.TokenHash,
                        ErrorCode.TokenNotFound,
                        new Dictionary<object, object>());
                }

                refreshToken.ApplyRefreshToke(command.UpdatedBy);
                _auow.WRefreshTokenRepository.Update(refreshToken);
                await _auow.CommitTransactionAsync(token);

                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to revoke RefreshToken");
                throw;
            }
        }
    }
}