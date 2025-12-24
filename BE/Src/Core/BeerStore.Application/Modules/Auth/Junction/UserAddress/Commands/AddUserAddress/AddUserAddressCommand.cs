using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Commands.AddUserAddress
{
    public record AddUserAddressCommand(Guid UserId, Guid AddressId) : IRequest<UserAddressResponse>;
}
