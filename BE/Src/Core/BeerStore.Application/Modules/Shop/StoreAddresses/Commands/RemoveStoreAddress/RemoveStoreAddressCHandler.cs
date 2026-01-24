using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.StoreAddresses.Commands.RemoveStoreAddress
{
    public class RemoveStoreAddressCHandler : IRequestHandler<RemoveStoreAddressCommand, bool>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<RemoveStoreAddressCHandler> _logger;

        public RemoveStoreAddressCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<RemoveStoreAddressCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveStoreAddressCommand command, CancellationToken token)
        {
            await _suow.BeginTransactionAsync(token);

            try
            {
                var address = await _suow.RStoreAddressRepository.GetByIdAsync(command.StoreAddressId, token);
                if (address == null)
                {
                    throw new BusinessRuleException<StoreAddressField>(
                        ErrorCategory.NotFound,
                        StoreAddressField.Id,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object> { { "StoreAddressId", command.StoreAddressId } });
                }

                await _authService.EnsureCanManageStoreAddress(address.StoreId);

                // Hard delete
                _suow.WStoreAddressRepository.Remove(address);

                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("StoreAddress {AddressId} removed", address.Id);

                return true;
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to remove store address {AddressId}", command.StoreAddressId);
                throw;
            }
        }
    }
}
