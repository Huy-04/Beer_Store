using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Shop.StoreAddress.Requests
{
    public record UpdateStoreAddressRequest(
        string Phone,
        string ContactName,
        string Province,
        string District,
        string Ward,
        string Street,
        int Type,
        StatusEnum IsDefault);
}
