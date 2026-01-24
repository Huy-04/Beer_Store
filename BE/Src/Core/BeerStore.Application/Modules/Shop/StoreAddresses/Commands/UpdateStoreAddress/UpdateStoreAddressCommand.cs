using BeerStore.Application.DTOs.Shop.StoreAddress.Requests;
using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.StoreAddresses.Commands.UpdateStoreAddress
{
    public record UpdateStoreAddressCommand(
        Guid StoreAddressId,
        Guid UpdatedBy,
        UpdateStoreAddressRequest Request) : IRequest<StoreAddressResponse>;
}
