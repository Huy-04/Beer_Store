using MediatR;

namespace BeerStore.Application.Modules.Shop.StoreAddresses.Commands.RemoveStoreAddress
{
    public record RemoveStoreAddressCommand(Guid StoreAddressId, Guid DeletedBy) : IRequest<bool>;
}
