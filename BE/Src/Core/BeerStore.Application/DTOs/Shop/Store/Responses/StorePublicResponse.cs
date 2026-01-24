namespace BeerStore.Application.DTOs.Shop.Store.Responses
{
    public record StorePublicResponse(
        Guid Id,
        string Name,
        string Slug,
        string? Logo,
        string? Description,
        bool IsOfficial);
}
