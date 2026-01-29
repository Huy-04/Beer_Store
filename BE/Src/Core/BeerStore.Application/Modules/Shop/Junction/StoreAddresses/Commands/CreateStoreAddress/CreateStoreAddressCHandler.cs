using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreAddressMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.CreateStoreAddress
{
    public class CreateStoreAddressCHandler : IRequestHandler<CreateStoreAddressCommand, StoreAddressResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<CreateStoreAddressCHandler> _logger;

        public CreateStoreAddressCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<CreateStoreAddressCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreAddressResponse> Handle(CreateStoreAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanManageStoreAddress(command.StoreId);

            await _suow.BeginTransactionAsync(token);

            try
            {
                // Verify store exists
                var store = await _suow.RStoreRepository.GetByIdAsync(command.StoreId, token);
                if (store == null)
                {
                    throw new Exception("Store not found");
                }

                var address = command.Request.ToStoreAddress(command.StoreId);
                store.AddAddress(address);
                _suow.WStoreRepository.Update(store);

                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("StoreAddress {AddressId} created for Store {StoreId}", address.Id, command.StoreId);

                return address.ToStoreAddressResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to create store address for Store {StoreId}", command.StoreId);
                throw;
            }
        }
    }
}
