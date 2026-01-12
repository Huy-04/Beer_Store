using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Addresses.Queries.GetAddressByPhone
{
    public record GetAddressByPhoneQuery(string Phone) : IRequest<IEnumerable<AddressResponse>>
    {
    }
}
