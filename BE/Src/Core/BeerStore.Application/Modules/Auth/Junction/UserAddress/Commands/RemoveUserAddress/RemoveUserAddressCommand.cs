using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Commands.RemoveUserAddress
{
    public record RemoveUserAddressCommand(Guid UserAddressId) : IRequest<Unit>;
}
