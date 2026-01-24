using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.BanStore
{
    public record BanStoreCommand(Guid StoreId, Guid UpdatedBy, string Reason) : IRequest<StoreResponse>;
}
