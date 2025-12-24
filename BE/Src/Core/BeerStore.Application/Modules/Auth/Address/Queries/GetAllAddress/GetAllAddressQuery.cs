using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Address.Queries.GetAllAddress
{
    public record GetAllAddressQuery() : IRequest<IEnumerable<AddressResponse>>
    {
    }
}
