using BeerStore.Application.DTOs.Auth.Authentication.Responses.Login;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.RefreshAccessToken
{
    public class RefreshAccessTokenCHandler : IRequestHandler<RefreshAccessTokenCommand, LoginResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IJwtService _jwtService;
        private readonly ILogger<RefreshAccessTokenCHandler> _logger;

        public RefreshAccessTokenCHandler(
            IAuthUnitOfWork auow,
            IJwtService jwtService,
            ILogger<RefreshAccessTokenCHandler> logger)
        {
            _auow = auow;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(RefreshAccessTokenCommand command, CancellationToken token)
        {
            try
            {
                var hashedToken = _jwtService.HashRefreshToken(command.RefreshToken);

                var refreshToken = await _auow.RRefreshTokenRepository.FindTokenAsync(token,
                    tokenHash: hashedToken, deviceId: command.DeviceId);

                if (refreshToken == null)
                {
                    _logger.LogWarning("Token not found for DeviceId {DeviceId}", command.DeviceId);
                    throw new BusinessRuleException<RefreshTokenField>(
                        ErrorCategory.NotFound,
                        RefreshTokenField.TokenHash,
                        ErrorCode.TokenNotFound,
                        new Dictionary<object, object>());
                }

                if (refreshToken.TokenStatus.Value != StatusEnum.Active)
                {
                    _logger.LogWarning("Token revoked for UserId {UserId}", refreshToken.UserId);
                    throw new BusinessRuleException<RefreshTokenField>(
                        ErrorCategory.Forbidden,
                        RefreshTokenField.TokenStatus,
                        ErrorCode.TokenRevoked,
                        new Dictionary<object, object>());
                }

                if (refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
                {
                    _logger.LogWarning("Token expired for UserId {UserId}", refreshToken.UserId);
                    throw new BusinessRuleException<RefreshTokenField>(
                        ErrorCategory.Unauthorized,
                        RefreshTokenField.ExpiresAt,
                        ErrorCode.TokenExpired,
                        new Dictionary<object, object>());
                }

                var user = await _auow.RUserRepository.GetByIdWithRolesAsync(refreshToken.UserId, token);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found", refreshToken.UserId);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.NotFound,
                        UserField.IdUser,
                        ErrorCode.UserNotFound,
                        new Dictionary<object, object>());
                }

                if (user.UserStatus.Value != StatusEnum.Active)
                {
                    _logger.LogWarning("User {UserId} inactive", user.Id);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Forbidden,
                        UserField.UserStatus,
                        ErrorCode.AccountInactive,
                        new Dictionary<object, object>());
                }

                var roleIds = user.UserRoles.Select(ur => ur.RoleId);
                var rolesEntities = await _auow.RRoleRepository.GetRolesByIdsAsync(roleIds, token);
                var roles = rolesEntities.Select(r => r.RoleName.Value).ToList();

                // Generate AccessToken
                var accessToken = _jwtService.GenerateToken(user.Id, user.Email, roles);

                return new LoginResponse(
                    accessToken,
                    user.Id,
                    user.Email.Value,
                    user.UserName.Value,
                    roles,
                    command.RefreshToken,
                    refreshToken.ExpiresAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh token. DeviceId: {DeviceId}", command.DeviceId);
                throw;
            }
        }
    }
}