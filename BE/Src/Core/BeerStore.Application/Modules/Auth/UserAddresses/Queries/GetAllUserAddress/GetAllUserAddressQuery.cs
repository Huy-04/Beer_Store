using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetAllUserAddress
{
    public record GetAllUserAddressQuery() : IRequest<IEnumerable<UserAddressResponse>>
    {
    }
}
