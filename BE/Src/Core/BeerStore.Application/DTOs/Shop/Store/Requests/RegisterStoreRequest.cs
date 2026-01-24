namespace BeerStore.Application.DTOs.Shop.Store.Requests
{
    public record RegisterStoreRequest(
        string Name,
        string Slug,
        string? Logo,
        string? Description,
        int StoreType);
}
