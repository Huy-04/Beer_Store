using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeAllUserRefreshTokens
{
    public class RevokeAllUserRefreshTokensCHandler : IRequestHandler<RevokeAllUserRefreshTokensCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RevokeAllUserRefreshTokensCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RevokeAllUserRefreshTokensCHandler(IAuthUnitOfWork auow, ILogger<RevokeAllUserRefreshTokensCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task Handle(RevokeAllUserRefreshTokensCommand command, CancellationToken token)
        {
            _authService.EnsureCanRevokeRefreshToken(command.UserId);

            await _auow.BeginTransactionAsync(token);

            try
            {
                var activeTokens = await _auow.RRefreshTokenRepository.GetActiveByUserIdAsync(command.UserId, token);

                foreach (var refreshToken in activeTokens)
                {
                    refreshToken.RevokeBy(command.UpdatedBy);
                    _auow.WRefreshTokenRepository.Update(refreshToken);
                }

                await _auow.CommitTransactionAsync(token);
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
