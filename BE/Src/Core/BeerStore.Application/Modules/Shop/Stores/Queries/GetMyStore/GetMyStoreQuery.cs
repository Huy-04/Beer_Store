using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetMyStore
{
    public record GetMyStoreQuery(Guid OwnerId) : IRequest<StoreResponse?>;
}
