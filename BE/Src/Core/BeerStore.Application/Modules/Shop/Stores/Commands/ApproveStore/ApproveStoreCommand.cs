using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.ApproveStore
{
    public record ApproveStoreCommand(Guid StoreId, Guid UpdatedBy) : IRequest<StoreResponse>;
}
