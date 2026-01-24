using BeerStore.Domain.Enums.Shop;

namespace BeerStore.Application.DTOs.Shop.Store.Responses
{
    public record StoreResponse(
        Guid Id,
        Guid OwnerId,
        string Name,
        string Slug,
        string Logo,
        string Description,
        StoreType StoreType,
        StoreStatus StoreStatus,
        bool IsOfficial,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);
}
