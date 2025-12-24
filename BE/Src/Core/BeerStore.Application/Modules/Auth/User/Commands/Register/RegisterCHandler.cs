using BeerStore.Application.DTOs.Auth.User.Responses.Register;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.Enums.Messages;
using BeerStore.Domain.ValueObjects.Auth.Roles;
using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Commands.Register
{
    public class RegisterCHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RegisterCHandler> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private const string Message = "User registered successfully";

        public RegisterCHandler(IAuthUnitOfWork auow, ILogger<RegisterCHandler> logger, IPasswordHasher passwordHasher)
        {
            _auow = auow;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var systemUser = await _auow.RUserRepository.GetByUserNameAsync(UserName.System);
                if (systemUser == null)
                {
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.NotFound,
                        UserField.UserName,
                        ErrorCode.UserNotFound,
                        new Dictionary<object, object> { { ParamField.Value, UserName.System.Value } });
                }

                var user = command.Request.ToUser(_passwordHasher, systemUser.Id, systemUser.Id);

                if (await _auow.RUserRepository.ExistsByUserNameAsync(user.UserName, token))
                {
                    _logger.LogWarning("Create failed: User with UserName '{UserName}' already exists", user.UserName.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Conflict,
                        UserField.UserName,
                        ErrorCode.UserNameAlreadyExists,
                        new Dictionary<object, object> { { ParamField.Value, user.UserName.Value } });
                }

                if (await _auow.RUserRepository.ExistsByEmailAsync(user.Email, token))
                {
                    _logger.LogWarning("Create failed: User with Email '{Email}' already exists", user.Email.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Conflict,
                        UserField.Email,
                        ErrorCode.EmailAlreadyExists,
                        new Dictionary<object, object> { { ParamField.Value, user.Email.Value } });
                }

                if (await _auow.RUserRepository.ExistsByPhoneAsync(user.Phone, token))
                {
                    _logger.LogWarning("Create failed: User with Phone '{Phone}' already exists", user.Phone.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Conflict,
                        UserField.Phone,
                        ErrorCode.PhoneAlreadyExists,
                        new Dictionary<object, object> { { ParamField.Value, user.Phone.Value } });
                }

                var roleName = RoleName.Customer;
                var role = await _auow.RRoleRepository.GetByNameAsync(roleName);

                if (role == null)
                {
                    throw new BusinessRuleException<RoleField>(
                        ErrorCategory.NotFound,
                        RoleField.RoleName,
                        ErrorCode.RoleNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value, roleName.Value },
                        });
                }
                user.AddRole(user.Id, role.Id);

                await _auow.WUserRepository.AddAsync(user);
                await _auow.CommitTransactionAsync(token);

                var register = new RegisterResponse(
                    user.Id,
                    Email.Create(user.Email),
                    UserName.Create(user.UserName),
                    FullName.Create(user.FullName),
                    user.CreatedAt,
                    Message);

                return register;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating User. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
