using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetStoreById
{
    public record GetStoreByIdQuery(Guid StoreId) : IRequest<StoreResponse?>;
}
