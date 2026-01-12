using BeerStore.Application.DTOs.Auth.Address.Requests;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Address;
using BeerStore.Domain.ValueObjects.Auth.User;

namespace BeerStore.Application.Mapping.Auth.AddressMap
{
    public static class RequestToAddress
    {
        public static Address ToAddress(this AddressRequest request, Guid userId, Guid createdBy, Guid updatedBy)
        {
            return Address.Create(
                userId,
                Phone.Create(request.Phone),
                FullName.Create(request.FullName),
                Province.Create(request.Province),
                District.Create(request.District),
                Ward.Create(request.Ward),
                Street.Create(request.Street),
                IsDefault.Create(request.IsDefault),
                AddressType.Create(request.AddressType),
                createdBy,
                updatedBy);
        }

        public static void ApplyAddress(this Address address, Guid updatedBy, AddressRequest request)
        {
            address.UpdatePhone(Phone.Create(request.Phone));
            address.UpdateFullName(FullName.Create(request.FullName));
            address.UpdateProvice(Province.Create(request.Province));
            address.UpdateDistrict(District.Create(request.District));
            address.UpdateWard(Ward.Create(request.Ward));
            address.UpdateStreet(Street.Create(request.Street));
            address.UpdateIsDefault(IsDefault.Create(request.IsDefault));
            address.UpdateAddressType(AddressType.Create(request.AddressType));
        }
    }
}
