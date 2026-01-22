using BeerStore.Application.DTOs.Auth.UserAddress.Requests;
using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Commands.UpdateUserAddress
{
    public record UpdateUserAddressCommand(Guid IdUserAddress, Guid UpdateBy, UserAddressRequest Request) : IRequest<UserAddressResponse>
    {
    }
}
