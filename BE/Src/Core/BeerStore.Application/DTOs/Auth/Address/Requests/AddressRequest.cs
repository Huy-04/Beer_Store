using BeerStore.Domain.Enums;
using BeerStore.Domain.ValueObjects.Auth.Address;
using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.Address.Requests
{
    public record AddressRequest(
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