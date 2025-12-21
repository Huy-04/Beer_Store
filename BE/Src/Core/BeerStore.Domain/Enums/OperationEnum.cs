using System.Text.Json.Serialization;

namespace BeerStore.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationEnum
    {
        read = 0,
        create = 1,
        update = 2,
        remove = 3
    }
}