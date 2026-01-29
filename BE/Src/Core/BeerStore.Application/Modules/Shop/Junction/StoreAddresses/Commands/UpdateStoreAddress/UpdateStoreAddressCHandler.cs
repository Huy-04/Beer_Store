using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreAddressMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.UpdateStoreAddress
{
    public class UpdateStoreAddressCHandler : IRequestHandler<UpdateStoreAddressCommand, StoreAddressResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<UpdateStoreAddressCHandler> _logger;

        public UpdateStoreAddressCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<UpdateStoreAddressCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreAddressResponse> Handle(UpdateStoreAddressCommand command, CancellationToken token)
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

                // 2. Load Store with Addresses
                var store = await _suow.RStoreRepository.GetByIdWithAddressesAsync(addressInfo.StoreId, token);
                if (store == null) throw new Exception("Store not found");

                // 3. Find/Update
                var address = store.StoreAddresses.FirstOrDefault(a => a.Id == command.StoreAddressId);
                if (address != null)
                {
                    address.ApplyUpdate(command.Request);
                    _suow.WStoreRepository.Update(store);
                    await _suow.CommitTransactionAsync(token);
                }

                _logger.LogInformation("StoreAddress {AddressId} updated", address.Id);

                return address.ToStoreAddressResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to update store address {AddressId}", command.StoreAddressId);
                throw;
            }
        }
    }
}