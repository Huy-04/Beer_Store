using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserAddressMap;
using BeerStore.Domain.Enums.Auth.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Commands.UpdateUserAddress
{
    public class UpdateUserAddressCHandler : IRequestHandler<UpdateUserAddressCommand, UserAddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateUserAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public UpdateUserAddressCHandler(IAuthUnitOfWork auow, ILogger<UpdateUserAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserAddressResponse> Handle(UpdateUserAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanUpdateAddress(command.IdUserAddress);

            await _auow.BeginTransactionAsync(token);

            try
            {
                // 1. Get Address Key to find User (via Read Repo)
                var addressInfo = await _auow.RUserAddressRepository.GetByIdAsync(command.IdUserAddress, token);
                 if (addressInfo == null)
                {
                    // Copy existing Not Found logic or use it here
                    _logger.LogWarning("UserAddress {Id} not found", command.IdUserAddress);
                    throw new BusinessRuleException<UserAddressField>(
                        ErrorCategory.NotFound,
                        UserAddressField.IdAddress,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.IdUserAddress }
                        });
                }

                // 2. Load User Aggregate with Addresses
                var user = await _auow.RUserRepository.GetByIdWithAddressesAsync(addressInfo.UserId, token);
                if (user == null) throw new Exception("User not found");

                // 3. Find address in aggregate
                var address = user.UserAddresses.FirstOrDefault(a => a.Id == command.IdUserAddress);
                if (address == null) throw new Exception("Address not found in User"); // Should not happen since we found it in step 1

                address.ApplyUserAddress(command.Request);
                
                // 4. Update Aggregate
                _auow.WUserRepository.Update(user);
                await _auow.CommitTransactionAsync(token);

                return address.ToUserAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to update UserAddress. Id: {Id}",
                    command.IdUserAddress
                );
                throw;
            }
        }
    }
}
