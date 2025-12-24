using MediatR;

namespace BeerStore.Application.Modules.Auth.Address.Commands.RemoveAddress
{
    public record RemoveAddressCommand(Guid IdAddress) : IRequest<bool>
    {
    }
}
