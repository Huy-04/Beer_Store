using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Mapping.Shop.StoreAddressMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Queries.GetStoreAddressesByStore
{
    public class GetStoreAddressesByStoreQHandler : IRequestHandler<GetStoreAddressesByStoreQuery, IEnumerable<StoreAddressResponse>>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly ILogger<GetStoreAddressesByStoreQHandler> _logger;

        public GetStoreAddressesByStoreQHandler(
            IShopUnitOfWork suow,
            ILogger<GetStoreAddressesByStoreQHandler> logger)
        {
            _suow = suow;
            _logger = logger;
        }

        public async Task<IEnumerable<StoreAddressResponse>> Handle(GetStoreAddressesByStoreQuery query, CancellationToken token)
        {
            // Public endpoint - addresses are public info for delivery/pickup
            var addresses = await _suow.RStoreAddressRepository.GetByStoreIdAsync(query.StoreId, token);

            _logger.LogDebug("Retrieved {Count} addresses for Store {StoreId}", addresses.Count(), query.StoreId);

            return addresses.ToStoreAddressResponses();
        }
    }
}
