using MediatR;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Commands.RemoveUserAddress
{
    public record RemoveUserAddressCommand(Guid IdUserAddress) : IRequest;

}
