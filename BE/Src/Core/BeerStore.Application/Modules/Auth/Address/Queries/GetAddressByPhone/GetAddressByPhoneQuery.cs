using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Address.Queries.GetAddressByPhone
{
    public record GetAddressByPhoneQuery(string Phone) : IRequest<IEnumerable<AddressResponse>>
    {
    }
}
