using BeerStore.Application.DTOs.Shop.Store.Requests;
using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.UpdateStore
{
    public record UpdateStoreCommand(
        Guid UpdatedBy,
        Guid StoreId,
        UpdateStoreRequest Request) : IRequest<StoreResponse>;
}
