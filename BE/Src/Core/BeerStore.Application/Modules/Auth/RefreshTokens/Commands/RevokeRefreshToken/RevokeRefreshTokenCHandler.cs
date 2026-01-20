using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
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
        private readonly IAuthAuthorizationService _authService;

        public RevokeRefreshTokenCHandler(IAuthUnitOfWork auow, ILogger<RevokeRefreshTokenCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<bool> Handle(RevokeRefreshTokenCommand command, CancellationToken token)
        {
            // Fetch first to get userId for authorization check
            var refreshToken = await _auow.RRefreshTokenRepository.GetByTokenHash(command.TokenHash, token);

            if (refreshToken == null)
            {
                _logger.LogWarning("RefreshToken not found for TokenHash");
                throw new BusinessRuleException<RefreshTokenField>(
                    ErrorCategory.NotFound,
                    RefreshTokenField.TokenHash,
                    ErrorCode.TokenNotFound,
                    new Dictionary<object, object>());
            }

            _authService.EnsureCanRevokeRefreshToken(refreshToken.UserId);

            await _auow.BeginTransactionAsync(token);

            try
            {
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
