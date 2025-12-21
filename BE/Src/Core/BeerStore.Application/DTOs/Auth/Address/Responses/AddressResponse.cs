using BeerStore.Domain.Enums;
using Domain.Core.Enums;

namespace BeerStore.Application.DTOs.Auth.Address.Responses
{
    public record AddressResponse(
        Guid IdAddress,
        string Phone,
        string FullName,
        string Province,
        string District,
        string Ward,
        string Street,
        StatusEnum IsDefault,
        AddressTypeEnum AddressType,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}