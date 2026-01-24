using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.SuspendStore
{
    public record SuspendStoreCommand(Guid StoreId, Guid UpdatedBy, string Reason) : IRequest<StoreResponse>;
}
