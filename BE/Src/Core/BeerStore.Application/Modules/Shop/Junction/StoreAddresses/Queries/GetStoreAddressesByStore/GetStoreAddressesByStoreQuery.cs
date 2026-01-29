using BeerStore.Application.DTOs.Shop.StoreAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Queries.GetStoreAddressesByStore
{
    public record GetStoreAddressesByStoreQuery(Guid StoreId) : IRequest<IEnumerable<StoreAddressResponse>>;
}
