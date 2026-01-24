using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.ReactivateStore
{
    public record ReactivateStoreCommand(Guid StoreId, Guid UpdatedBy) : IRequest<StoreResponse>;
}
