namespace BeerStore.Application.DTOs.Shop.Store.Requests
{
    public record ResubmitStoreRequest(
        string Name,
        string? Logo,
        string? Description);
}
