using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Commands.CreateUser
{
    public class CreateUserCHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<CreateUserCHandler> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCHandler(IAuthUnitOfWork auow, ILogger<CreateUserCHandler> logger, IPasswordHasher passwordHasher)
        {
            _auow = auow;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = command.Request.ToUser(_passwordHasher, command.CreatedBy, command.UpdatedBy);

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

                await _auow.WUserRepository.AddAsync(user, token);
                await _auow.CommitTransactionAsync(token);

                return user.ToUserResponse();
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