using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetAllStores
{
    public class GetAllStoresQHandler : IRequestHandler<GetAllStoresQuery, IEnumerable<StoreResponse>>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<GetAllStoresQHandler> _logger;

        public GetAllStoresQHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<GetAllStoresQHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<IEnumerable<StoreResponse>> Handle(GetAllStoresQuery query, CancellationToken token)
        {
            _authService.EnsureCanViewAllStores();

            var stores = await _suow.RStoreRepository.GetAllAsync(token);

            // Apply in-memory filtering for now (can be optimized with spec pattern later)
            var filteredStores = stores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                filteredStores = filteredStores.Where(s => 
                    s.Name.Value.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.Slug.Value.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                filteredStores = filteredStores.Where(s => 
                    s.StoreStatus.ToString().Equals(query.Status, StringComparison.OrdinalIgnoreCase));
            }

            var result = filteredStores
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            _logger.LogDebug("Retrieved {Count} stores for page {Page}", result.Count, query.PageNumber);

            return result.ToStoreResponses();
        }
    }
}
