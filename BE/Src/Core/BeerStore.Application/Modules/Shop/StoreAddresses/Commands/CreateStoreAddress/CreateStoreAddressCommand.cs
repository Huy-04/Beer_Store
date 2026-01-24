using BeerStore.Application.DTOs.Shop.StoreAddress.Requests;
using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.StoreAddresses.Commands.CreateStoreAddress
{
    public record CreateStoreAddressCommand(
        Guid StoreId,
        Guid CreatedBy,
        CreateStoreAddressRequest Request) : IRequest<StoreAddressResponse>;
}
