using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Domain.Entities.Auth;

namespace BeerStore.Application.Mapping.Auth.AddressMap
{
    public static class AddressToResponse
    {
        public static AddressResponse ToAddressResponse(this Address address)
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
                address.AddressType.Value,
                address.CreatedBy,
                address.UpdatedBy,
                address.CreatedAt,
                address.UpdatedAt);
        }
    }
}
