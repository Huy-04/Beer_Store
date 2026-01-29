using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Queries.GetUserAddressById
{
    public record GetUserAddressByIdQuery(Guid Id) : IRequest<UserAddressResponse>;
}
