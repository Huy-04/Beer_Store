using BeerStore.Application.DTOs.Auth.User.Responses.Login;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Commands.Login
{
    public class LoginCHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginCHandler> _logger;

        public LoginCHandler(IAuthUnitOfWork auow, IPasswordHasher passwordHasher, IJwtService jwtService, ILogger<LoginCHandler> logger)
        {
            _auow = auow;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken token)
        {
            var request = command.Request;

            var allUsers = await _auow.RUserRepository.GetAllWithRolesAsync(token);
            var user = allUsers.FirstOrDefault(u =>
                u.Email.Value == request.Email &&
                u.UserStatus.Value == StatusEnum.Active);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User with Email '{Email}' not found or inactive", request.Email);
                throw new BusinessRuleException<UserField>(
                    ErrorCategory.Unauthorized,
                    UserField.Email,
                    ErrorCode.UserNotFound,
                    new Dictionary<object, object>
                    {
                        { ParamField.Value, request.Email },
                    });
            }

            if (user.UserStatus.Value != StatusEnum.Active)
            {
                _logger.LogWarning("Login failed: User account is inactive. Email: '{Email}', Status: {Status}", request.Email, user.UserStatus.Value);
                throw new BusinessRuleException<UserField>(
                    ErrorCategory.Unauthorized,
                    UserField.UserStatus,
                    ErrorCode.AccountInactive,
                    new Dictionary<object, object>
                    {
                        { ParamField.Value, user.UserStatus.Value.ToString() },
                    });
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.Password.Value))
            {
                _logger.LogWarning("Login failed: Invalid password for Email '{Email}'", request.Email);
                throw new BusinessRuleException<UserField>(
                    ErrorCategory.Unauthorized,
                    UserField.Password,
                    ErrorCode.InvalidPassword,
                    new Dictionary<object, object>
                    {
                        { ParamField.Value, ParamField.InvalidPassword.ToString() },
                    });
            }

            // Get user roles
            var roles = new List<string>();
            foreach (var userRole in user.UserRoles)
            {
                var role = await _auow.RRoleRepository.GetByIdAsync(userRole.RoleId, token);
                if (role != null)
                {
                    roles.Add(role.RoleName.Value);
                }
            }

            // Generate JWT token
            var jwtToken = _jwtService.GenerateToken(user.Id, user.Email, roles);

            return new LoginResponse(
                Token: jwtToken,
                UserId: user.Id,
                Email: user.Email.Value,
                UserName: user.UserName.Value,
                Roles: roles);
        }
    }
}