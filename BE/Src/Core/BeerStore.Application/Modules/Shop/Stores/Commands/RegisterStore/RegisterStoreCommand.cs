using BeerStore.Application.DTOs.Shop.Store.Requests;
using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.RegisterStore
{
    public record RegisterStoreCommand(
        Guid CreatedBy,
        Guid UpdatedBy,
        RegisterStoreRequest Request) : IRequest<StoreResponse>;
}
