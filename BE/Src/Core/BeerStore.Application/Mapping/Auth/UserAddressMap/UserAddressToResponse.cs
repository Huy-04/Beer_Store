using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.UserAddressMap
{
    public static class UserAddressToResponse
    {
        public static UserAddressResponse ToUserAddressResponse(this UserAddress address)
        {
            return new(
                address.Id,
                address.Phone.Value,
                address.FullName.Value,
                address.Province.Value,
                address.District.Value,
                address.Ward.Value,
                address.Street.Value,
                address.IsDefault.Value,
                address.UserAddressType.Value,
                address.CreatedBy,
                address.UpdatedBy,
                address.CreatedAt,
                address.UpdatedAt);
        }
    }
}