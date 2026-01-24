using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.RejectStore
{
    public record RejectStoreCommand(Guid StoreId, Guid UpdatedBy, string? Reason) : IRequest<StoreResponse>;
}
