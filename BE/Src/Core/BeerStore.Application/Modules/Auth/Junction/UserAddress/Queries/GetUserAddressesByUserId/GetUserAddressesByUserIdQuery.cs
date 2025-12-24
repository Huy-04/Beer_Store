using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Queries.GetUserAddressesByUserId
{
    public record GetUserAddressesByUserIdQuery(Guid UserId) : IRequest<IEnumerable<UserAddressResponse>>;
}
