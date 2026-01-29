using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Commands.RemoveUserAddress
{
    public class RemoveUserAddressCHandler : IRequestHandler<RemoveUserAddressCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveUserAddressCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<bool> Handle(RemoveUserAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanRemoveAddress(command.UserAddressId); // Fixed property name

            await _auow.BeginTransactionAsync(token);

            try
            {
                // 1. Get Address to find User
                var addressInfo = await _auow.RUserAddressRepository.GetByIdAsync(command.UserAddressId, token);
                
                if (addressInfo == null)
                {
                    _logger.LogWarning("UserAddress {Id} not found", command.UserAddressId);
                    throw new BusinessRuleException<UserAddressField>(
                        ErrorCategory.NotFound,
                        UserAddressField.IdAddress,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.UserAddressId }
                        });
                }

                // 2. Load User with Addresses
                var user = await _auow.RUserRepository.GetByIdWithAddressesAsync(addressInfo.UserId, token);

                if (user != null)
                {
                    user.RemoveAddress(command.UserAddressId);
                    _auow.WUserRepository.Update(user);
                    await _auow.CommitTransactionAsync(token);
                }
                else
                {
                    // Handle orphan address case if needed, or just commit.
                    await _auow.CommitTransactionAsync(token);
                }

                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove UserAddress. Id: {Id}",
                    command.UserAddressId
                );
                throw;
            }
        }
    }
}
