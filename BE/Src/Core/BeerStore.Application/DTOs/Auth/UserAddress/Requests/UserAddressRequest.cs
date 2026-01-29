using BeerStore.Domain.Enums;
using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.UserAddress.Requests
{
    public record UserAddressRequest(
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