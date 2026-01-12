using Application.Core.Interface.ISettings;
using BeerStore.Application.DTOs.Auth.RefreshToken.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.CreateRefreshToken
{
    public class CreateRefreshTokenCHandler : IRequestHandler<CreateRefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IJwtService _jwtService;
        private readonly ILogger<CreateRefreshTokenCHandler> _logger;
        private readonly IJwtSettings _jwtSetting;

        public CreateRefreshTokenCHandler(IAuthUnitOfWork auow,
            IJwtService jwtService, ILogger<CreateRefreshTokenCHandler> logger,
            IJwtSettings jwtSettings)
        {
            _auow = auow;
            _jwtService = jwtService;
            _logger = logger;
            _jwtSetting = jwtSettings;
        }

        public async Task<RefreshTokenResponse> Handle(CreateRefreshTokenCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                // Generate new refresh token
                var rawToken = _jwtService.GenerateRefreshToken();
                var hashedToken = _jwtService.HashRefreshToken(rawToken);
                var tokenHash = TokenHash.Create(hashedToken);
                var expiresAt = DateTimeOffset.UtcNow.AddDays(_jwtSetting.RefreshTokenExpirationDays);

                var refreshToken = command.Request.ToRefreshToken(command.CreatedBy, command.UpdatedBy, tokenHash, expiresAt);

                await _auow.WRefreshTokenRepository.AddAsync(refreshToken, token);
                await _auow.CommitTransactionAsync(token);

                return refreshToken.ToRefreshTokenResponse(rawToken);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to create RefreshToken. UserId: {UserId}, DeviceId: {DeviceId}",
                    command.Request.userId, command.Request.deviceId);
                throw;
            }
        }
    }
}