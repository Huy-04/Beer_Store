using MediatR;

namespace BeerStore.Application.Modules.Auth.Addresses.Commands.RemoveAddress
{
    public record RemoveAddressCommand(Guid IdAddress) : IRequest<bool>
    {
    }
}
