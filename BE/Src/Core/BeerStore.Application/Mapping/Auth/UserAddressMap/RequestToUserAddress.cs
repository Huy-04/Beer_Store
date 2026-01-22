using BeerStore.Application.DTOs.Auth.UserAddress.Requests;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.Address;
using BeerStore.Domain.ValueObjects.Auth.User;

namespace BeerStore.Application.Mapping.Auth.UserAddressMap
{
    public static class RequestToUserAddress
    {
        public static UserAddress ToUserAddress(this UserAddressRequest request, Guid userId, Guid createdBy, Guid updatedBy)
        {
            return UserAddress.Create(
                userId,
                Phone.Create(request.Phone),
                FullName.Create(request.FullName),
                Province.Create(request.Province),
                District.Create(request.District),
                Ward.Create(request.Ward),
                Street.Create(request.Street),
                IsDefault.Create(request.IsDefault),
                UserAddressType.Create(request.AddressType),
                createdBy,
                updatedBy);
        }

        public static void ApplyUserAddress(this UserAddress address, Guid updatedBy, UserAddressRequest request)
        {
            address.UpdatePhone(Phone.Create(request.Phone));
            address.UpdateFullName(FullName.Create(request.FullName));
            address.UpdateProvice(Province.Create(request.Province));
            address.UpdateDistrict(District.Create(request.District));
            address.UpdateWard(Ward.Create(request.Ward));
            address.UpdateStreet(Street.Create(request.Street));
            address.UpdateIsDefault(IsDefault.Create(request.IsDefault));
            address.UpdateAddressType(UserAddressType.Create(request.AddressType));
        }
    }
}
