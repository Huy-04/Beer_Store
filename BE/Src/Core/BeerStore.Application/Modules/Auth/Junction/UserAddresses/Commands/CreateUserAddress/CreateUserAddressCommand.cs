using BeerStore.Application.DTOs.Auth.UserAddress.Requests;
using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Commands.CreateUserAddress
{
    public record CreateUserAddressCommand(Guid UserId, Guid CreatedBy, Guid UpdateBy, UserAddressRequest Request) : IRequest<UserAddressResponse>
    {
    }
}
