using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Addresses.Queries.GetAllAddress
{
    public record GetAllAddressQuery() : IRequest<IEnumerable<AddressResponse>>
    {
    }
}
