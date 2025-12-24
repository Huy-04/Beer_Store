using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;

namespace BeerStore.Application.Mapping.Auth.Junction.UserAddressMap
{
    public static class UserAddressToResponse
    {
        public static UserAddressResponse ToUserAddressResponse(this UserAddress userAddress)
        {
            return new UserAddressResponse(userAddress.Id, userAddress.UserId, userAddress.AddressId);
        }
    }
}
