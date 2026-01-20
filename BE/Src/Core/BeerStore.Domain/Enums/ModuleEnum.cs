using System.Text.Json.Serialization;

namespace BeerStore.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ModuleEnum
    {
        user = 0,
        roles = 1,
        permissions = 2,
        address = 3,
        refreshtoken = 4
    }
}