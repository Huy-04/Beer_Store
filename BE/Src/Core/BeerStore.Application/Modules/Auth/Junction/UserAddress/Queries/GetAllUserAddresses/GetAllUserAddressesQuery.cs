using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Queries.GetAllUserAddresses
{
    public record GetAllUserAddressesQuery : IRequest<IEnumerable<UserAddressResponse>>;
}
