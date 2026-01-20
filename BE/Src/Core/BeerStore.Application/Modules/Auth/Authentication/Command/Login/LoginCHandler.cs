using Application.Core.Interface.ISettings;
using BeerStore.Application.DTOs.Auth.Authentication.Responses.Login;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.Enums.Messages;
using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Authentication.Command.Login
{
    public class LoginCHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginCHandler> _logger;
        private readonly IJwtSettings _jwtSettings;

        public LoginCHandler(IAuthUnitOfWork auow, IPasswordHasher passwordHasher,
            IJwtService jwtService, ILogger<LoginCHandler> logger, IJwtSettings jwtSettings)
        {
            _auow = auow;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _logger = logger;
            _jwtSettings = jwtSettings;
        }

        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken token)
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

                var request = command.Request;

                var user = await _auow.RUserRepository.GetByEmailWithRolesAsync(
                    Email.Create(request.Email), token);

                if (user == null)
                {
                    _logger.LogWarning("User with Email {Email} not found", request.Email);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.NotFound,
                        UserField.Email,
                        ErrorCode.UserNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, request.Email },
                        });
                }

                if (user.UserStatus.Value != StatusEnum.Active)
                {
                    _logger.LogWarning("User account inactive. Email: {Email}, Status: {Status}", request.Email, user.UserStatus.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Forbidden,
                        UserField.UserStatus,
                        ErrorCode.AccountInactive,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, user.UserStatus.Value },
                        });
                }

                if (!_passwordHasher.VerifyPassword(request.Password, user.Password.Value))
                {
                    _logger.LogWarning("Invalid password for Email {Email}", request.Email);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Unauthorized,
                        UserField.Password,
                        ErrorCode.InvalidPassword,
                        new Dictionary<object, object>());
                }

                var roleIds = user.UserRoles.Select(ur => ur.RoleId);
                var rolesEntities = await _auow.RRoleRepository.GetRolesByIdsAsync(roleIds, token);
                var roles = rolesEntities.Select(r => r.RoleName.Value).ToList();

                // Get permissions for roles
                var permissions = (await _auow.RPermissionRepository
                    .GetPermissionNamesByRoleIdsAsync(roleIds, token)).ToList();

                // Revoke existing token on same device (if exists)
                var existingToken = await _auow.RRefreshTokenRepository.
                    GetByUserIdAndDeviceIdAsync(user.Id, command.DeviceId, token);
                if (existingToken != null)
                {
                    existingToken.ApplyRefreshToke(userSystem.Id);
                    _auow.WRefreshTokenRepository.Update(existingToken);
                }

                // Generate JWT token
                var jwtToken = _jwtService.GenerateToken(user.Id, user.Email, roles, permissions);

                // Generate Refresh Token
                var rawRefreshToken = _jwtService.GenerateRefreshToken();
                var hashedToken = _jwtService.HashRefreshToken(rawRefreshToken);

                var refreshToken = RefreshToken.Create(
                    user.Id,
                    TokenHash.Create(hashedToken),
                    DeviceId.Create(command.DeviceId),
                    DeviceName.Create(command.DeviceName),
                    IpAddress.Create(command.IpAddress),
                    DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                    userSystem.Id,
                    userSystem.Id);

                await _auow.WRefreshTokenRepository.AddAsync(refreshToken);
                await _auow.CommitTransactionAsync(token);

                return new LoginResponse(
                    jwtToken,
                    user.Id,
                    user.Email.Value,
                    user.UserName.Value,
                    roles,
                    rawRefreshToken,
                    refreshToken.ExpiresAt);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to login. Email: {Email}", command.Request.Email);
                throw;
            }
        }
    }
}