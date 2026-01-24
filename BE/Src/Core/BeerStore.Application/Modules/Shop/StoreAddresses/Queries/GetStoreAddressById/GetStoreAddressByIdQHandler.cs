using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Mapping.Shop.StoreAddressMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.StoreAddresses.Queries.GetStoreAddressById
{
    public class GetStoreAddressByIdQHandler : IRequestHandler<GetStoreAddressByIdQuery, StoreAddressResponse?>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly ILogger<GetStoreAddressByIdQHandler> _logger;

        public GetStoreAddressByIdQHandler(
            IShopUnitOfWork suow,
            ILogger<GetStoreAddressByIdQHandler> logger)
        {
            _suow = suow;
            _logger = logger;
        }

        public async Task<StoreAddressResponse?> Handle(GetStoreAddressByIdQuery query, CancellationToken token)
        {
            // Public endpoint - address details are public info
            var address = await _suow.RStoreAddressRepository.GetByIdAsync(query.StoreAddressId, token);

            if (address == null)
            {
                _logger.LogDebug("StoreAddress {AddressId} not found", query.StoreAddressId);
                return null;
            }

            return address.ToStoreAddressResponse();
        }
    }
}
