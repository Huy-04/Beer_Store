using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Queries.GetAllUserAddress
{
    public record GetAllUserAddressQuery(Guid UserId) : IRequest<IEnumerable<UserAddressResponse>>;
}
