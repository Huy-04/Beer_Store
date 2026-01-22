using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetUserAddressByPhone
{
    public record GetUserAddressByPhoneQuery(string Phone) : IRequest<IEnumerable<UserAddressResponse>>
    {
    }
}
