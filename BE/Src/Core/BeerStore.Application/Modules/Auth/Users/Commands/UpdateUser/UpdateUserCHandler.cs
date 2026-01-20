using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.Enums.Messages;
using BeerStore.Domain.ValueObjects.Auth.User;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Users.Commands.UpdateUser
{
    public class UpdateUserCHandler : IRequestHandler<UpdateUserCommand, UserResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateUserCHandler> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthAuthorizationService _authService;

        public UpdateUserCHandler(IAuthUnitOfWork auow, ILogger<UpdateUserCHandler> logger, IPasswordHasher passwordHasher, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        public async Task<UserResponse> Handle(UpdateUserCommand command, CancellationToken token)
        {
            _authService.EnsureCanUpdateUser(command.IdUser);

            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = await _auow.RUserRepository.GetByIdAsync(command.IdUser, token);

                if (user == null)
                {
                    _logger.LogWarning("User {Id} not found", command.IdUser);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.NotFound,
                        UserField.IdUser,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            {ParamField.Value, command.IdUser },
                        });
                }

                var newUserName = UserName.Create(command.Request.UserName);

                if (await _auow.RUserRepository.ExistsByUserNameAsync(newUserName, token, user.Id))
                {
                    _logger.LogWarning("User with UserName {UserName} already exists", newUserName.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Conflict,
                        UserField.UserName,
                        ErrorCode.UserNameAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value, newUserName.Value }
                        });
                }

                var newEmail = Email.Create(command.Request.Email);

                if (await _auow.RUserRepository.ExistsByEmailAsync(newEmail, token, user.Id))
                {
                    _logger.LogWarning("User with Email {Email} already exists", newEmail.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Conflict,
                        UserField.Email,
                        ErrorCode.EmailAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value, newEmail.Value }
                        });
                }

                var newPhone = Phone.Create(command.Request.Phone);

                if (await _auow.RUserRepository.ExistsByPhoneAsync(newPhone, token, user.Id))
                {
                    _logger.LogWarning("User with Phone {Phone} already exists", newPhone.Value);
                    throw new BusinessRuleException<UserField>(
                        ErrorCategory.Conflict,
                        UserField.Phone,
                        ErrorCode.PhoneAlreadyExists,
                        new Dictionary<object, object> {
                            { ParamField.Value, newPhone.Value }
                        });
                }

                user.ApplyUser(_passwordHasher, command.Request, command.UpdatedBy);

                _auow.WUserRepository.Update(user);
                await _auow.CommitTransactionAsync(token);

                return user.ToUserResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to update User. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}

