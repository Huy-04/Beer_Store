using BeerStore.Application.DTOs.Shop.Store.Requests;
using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.ResubmitStore
{
    public record ResubmitStoreCommand(
        Guid StoreId,
        Guid UpdatedBy,
        ResubmitStoreRequest Request) : IRequest<StoreResponse>;
}
