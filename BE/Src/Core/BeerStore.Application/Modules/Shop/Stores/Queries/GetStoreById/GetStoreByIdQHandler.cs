using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetStoreById
{
    public class GetStoreByIdQHandler : IRequestHandler<GetStoreByIdQuery, StoreResponse?>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<GetStoreByIdQHandler> _logger;

        public GetStoreByIdQHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<GetStoreByIdQHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse?> Handle(GetStoreByIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanViewStore(query.StoreId);

            var store = await _suow.RStoreRepository.GetByIdAsync(query.StoreId, token);
            
            if (store == null)
            {
                _logger.LogDebug("Store {StoreId} not found", query.StoreId);
                return null;
            }

            return store.ToStoreResponse();
        }
    }
}
