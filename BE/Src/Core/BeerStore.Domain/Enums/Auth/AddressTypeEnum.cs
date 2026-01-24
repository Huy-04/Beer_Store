using System.Text.Json.Serialization;

namespace BeerStore.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AddressTypeEnum
    {
        Home = 0,
        office = 1
    }
}
