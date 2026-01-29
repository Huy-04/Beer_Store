using BeerStore.Domain.Enums.Shop;
using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Shop.StoreAddress.Responses
{
    public record StoreAddressResponse(
        Guid Id,
        Guid StoreId,
        string Phone,
        string ContactName,
        string Province,
        string District,
        string Ward,
        string Street,
        StoreAddressType Type,
        StatusEnum IsDefault);
}
