using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.RemoveStoreAddress
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
                // 1. Get Address to find Store
                var addressInfo = await _suow.RStoreAddressRepository.GetByIdAsync(command.StoreAddressId, token);
                if (addressInfo == null)
                {
                    _logger.LogWarning("StoreAddress {Id} not found", command.StoreAddressId);
                    throw new BusinessRuleException<StoreAddressField>(
                        ErrorCategory.NotFound,
                        StoreAddressField.Id,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { "StoreAddressId", command.StoreAddressId }
                        });
                }

                // 2. Load Store
                var store = await _suow.RStoreRepository.GetByIdWithAddressesAsync(addressInfo.StoreId, token);
                if (store != null)
                {
                    await _authService.EnsureCanManageStoreAddress(store.Id); // Authorization check for the store
                    store.RemoveAddress(command.StoreAddressId);
                    _suow.WStoreRepository.Update(store);
                    await _suow.CommitTransactionAsync(token);
                }
                else
                {
                    // This case should ideally not happen if addressInfo was found,
                    // unless the store was deleted concurrently.
                    // For robustness, we can log and potentially throw an error or handle it.
                    _logger.LogWarning("Store {StoreId} not found for address {AddressId}", addressInfo.StoreId, addressInfo.Id);
                    throw new BusinessRuleException<StoreAddressField>(
                        ErrorCategory.NotFound,
                        StoreAddressField.StoreId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { "StoreId", addressInfo.StoreId }
                        });
                }

                _logger.LogInformation("StoreAddress {AddressId} removed", addressInfo.Id);

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
