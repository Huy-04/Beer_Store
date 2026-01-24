using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetAllStores
{
    public record GetAllStoresQuery(
        int PageNumber = 1,
        int PageSize = 20,
        string? SearchTerm = null,
        string? Status = null) : IRequest<IEnumerable<StoreResponse>>;
}
