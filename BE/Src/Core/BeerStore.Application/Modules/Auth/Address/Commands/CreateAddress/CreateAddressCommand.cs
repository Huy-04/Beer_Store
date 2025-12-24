using BeerStore.Application.DTOs.Auth.Address.Requests;
using BeerStore.Application.DTOs.Auth.Address.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Address.Commands.CreateAddress
{
    public record CreateAddressCommand(Guid UserId, Guid CreatedBy, Guid UpdateBy, AddressRequest Request) : IRequest<AddressResponse>
    {
    }
}
