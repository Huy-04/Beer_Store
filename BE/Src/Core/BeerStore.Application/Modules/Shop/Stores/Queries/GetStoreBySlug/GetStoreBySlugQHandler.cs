using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Mapping.Shop.StoreMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetStoreBySlug
{
    public class GetStoreBySlugQHandler : IRequestHandler<GetStoreBySlugQuery, StorePublicResponse?>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly ILogger<GetStoreBySlugQHandler> _logger;

        public GetStoreBySlugQHandler(
            IShopUnitOfWork suow,
            ILogger<GetStoreBySlugQHandler> logger)
        {
            _suow = suow;
            _logger = logger;
        }

        public async Task<StorePublicResponse?> Handle(GetStoreBySlugQuery query, CancellationToken token)
        {
            // Public endpoint - no authorization required
            var store = await _suow.RStoreRepository.GetBySlugAsync(query.Slug, token);
            
            if (store == null)
            {
                _logger.LogDebug("Store with slug '{Slug}' not found", query.Slug);
                return null;
            }

            return store.ToStorePublicResponse();
        }
    }
}
