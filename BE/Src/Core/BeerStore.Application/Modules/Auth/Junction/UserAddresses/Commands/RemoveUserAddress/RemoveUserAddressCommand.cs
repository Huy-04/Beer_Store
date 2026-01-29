using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Commands.RemoveUserAddress
{
    public record RemoveUserAddressCommand(Guid UserAddressId, Guid DeletedBy) : IRequest<bool>;

}
