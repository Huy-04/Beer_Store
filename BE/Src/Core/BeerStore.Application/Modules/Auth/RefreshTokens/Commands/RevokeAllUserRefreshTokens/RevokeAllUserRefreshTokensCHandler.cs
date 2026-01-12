using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeAllUserRefreshTokens
{
    public class RevokeAllUserRefreshTokensCHandler : IRequestHandler<RevokeAllUserRefreshTokensCommand, int>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RevokeAllUserRefreshTokensCHandler> _logger;

        public RevokeAllUserRefreshTokensCHandler(IAuthUnitOfWork auow, ILogger<RevokeAllUserRefreshTokensCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<int> Handle(RevokeAllUserRefreshTokensCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var activeTokens = await _auow.RRefreshTokenRepository.GetActiveByUserIdAsync(command.UserId, token);
                var revokedCount = 0;

                foreach (var refreshToken in activeTokens)
                {
                    refreshToken.ApplyRefreshToke(command.UpdatedBy);
                    _auow.WRefreshTokenRepository.Update(refreshToken);
                    revokedCount++;
                }

                await _auow.CommitTransactionAsync(token);

                return revokedCount;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to revoke all RefreshTokens. UserId: {UserId}", command.UserId);
                throw;
            }
        }
    }
}