using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Mapping.Shop.StoreMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetMyStore
{
    public class GetMyStoreQHandler : IRequestHandler<GetMyStoreQuery, StoreResponse?>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly ILogger<GetMyStoreQHandler> _logger;

        public GetMyStoreQHandler(
            IShopUnitOfWork suow,
            ILogger<GetMyStoreQHandler> logger)
        {
            _suow = suow;
            _logger = logger;
        }

        public async Task<StoreResponse?> Handle(GetMyStoreQuery query, CancellationToken token)
        {
            // User can only view their own store - OwnerId comes from token
            var store = await _suow.RStoreRepository.GetByOwnerIdAsync(query.OwnerId, token);
            
            if (store == null)
            {
                _logger.LogDebug("Store for owner {OwnerId} not found", query.OwnerId);
                return null;
            }

            return store.ToStoreResponse();
        }
    }
}
