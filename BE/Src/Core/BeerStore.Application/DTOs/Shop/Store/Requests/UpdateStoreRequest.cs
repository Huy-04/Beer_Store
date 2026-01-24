namespace BeerStore.Application.DTOs.Shop.Store.Requests
{
    public record UpdateStoreRequest(
        string Name,
        string? Logo,
        string? Description);
}
