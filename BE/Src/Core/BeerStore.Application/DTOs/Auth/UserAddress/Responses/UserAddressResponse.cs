using BeerStore.Domain.Enums;
using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.UserAddress.Responses
{
    public record UserAddressResponse(
        Guid IdUserAddress,
        string Phone,
        string FullName,
        string Province,
        string District,
        string Ward,
        string Street,
        StatusEnum IsDefault,
        AddressTypeEnum AddressType)
    {
    }
}
