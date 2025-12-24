using BeerStore.Application.DTOs.Auth.Address.Requests;
using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Address.Commands.UpdateAddress
{
    public record UpdateAddressCommand(Guid IdAddress, Guid UpdateBy, AddressRequest Request) : IRequest<AddressResponse>
    {
    }
}
