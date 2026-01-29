using MediatR;

namespace BeerStore.Application.Modules.Shop.Junction.StoreAddresses.Commands.RemoveStoreAddress
{
    public record RemoveStoreAddressCommand(Guid StoreAddressId, Guid DeletedBy) : IRequest<bool>;
}
