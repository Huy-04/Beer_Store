using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using BeerStore.Domain.Entities.Shop;

namespace BeerStore.Application.Mapping.Shop.StoreAddressMap
{
    public static class StoreAddressToResponse
    {
        public static StoreAddressResponse ToStoreAddressResponse(this StoreAddress address)
        {
            return new StoreAddressResponse(
                address.Id,
                address.StoreId,
                address.Phone.Value,
                address.ContactName.Value,
                address.Province.Value,
                address.District.Value,
                address.Ward.Value,
                address.Street.Value,
                address.Type,
                address.IsDefault.Value,
                address.CreatedAt,
                address.UpdatedAt);
        }

        public static IEnumerable<StoreAddressResponse> ToStoreAddressResponses(this IEnumerable<StoreAddress> addresses)
        {
            return addresses.Select(a => a.ToStoreAddressResponse());
        }
    }
}
