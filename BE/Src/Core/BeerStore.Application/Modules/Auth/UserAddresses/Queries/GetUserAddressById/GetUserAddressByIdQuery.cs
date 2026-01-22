using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetUserAddressById
{
    public record GetUserAddressByIdQuery(Guid IdUserAddress) : IRequest<UserAddressResponse>
    {
    }
}
