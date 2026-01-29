using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Domain.Entities.Shop;

namespace BeerStore.Application.Mapping.Shop.StoreMap
{
    public static class StoreToResponse
    {
        public static StoreResponse ToStoreResponse(this Store store)
        {
            return new StoreResponse(
                store.Id,
                store.OwnerId,
                store.StoreName.Value,
                store.Slug.Value,
                store.Logo.Value,
                store.Description.Value,
                store.StoreType,
                store.StoreStatus,
                store.IsOfficial,
                store.CreatedBy,
                store.UpdatedBy,
                store.CreatedAt,
                store.UpdatedAt);
        }

        public static StorePublicResponse ToStorePublicResponse(this Store store)
        {
            return new StorePublicResponse(
                store.Id,
                store.StoreName.Value,
                store.Slug.Value,
                store.Logo.Value,
                store.Description.Value,
                store.IsOfficial);
        }

        public static IEnumerable<StoreResponse> ToStoreResponses(this IEnumerable<Store> stores)
        {
            return stores.Select(s => s.ToStoreResponse());
        }

        public static IEnumerable<StorePublicResponse> ToStorePublicResponses(this IEnumerable<Store> stores)
        {
            return stores.Select(s => s.ToStorePublicResponse());
        }
    }
}
