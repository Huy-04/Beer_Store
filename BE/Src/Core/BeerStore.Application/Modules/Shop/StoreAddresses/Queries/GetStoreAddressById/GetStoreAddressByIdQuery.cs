using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.StoreAddresses.Queries.GetStoreAddressById
{
    public record GetStoreAddressByIdQuery(Guid StoreAddressId) : IRequest<StoreAddressResponse?>;
}
