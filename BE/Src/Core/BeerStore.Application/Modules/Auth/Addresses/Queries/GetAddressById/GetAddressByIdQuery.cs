using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Addresses.Queries.GetAddressById
{
    public record GetAddressByIdQuery(Guid IdAddress) : IRequest<AddressResponse>
    {
    }
}
