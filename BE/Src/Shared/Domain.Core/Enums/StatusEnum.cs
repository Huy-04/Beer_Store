using System.Text.Json.Serialization;

namespace Domain.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusEnum
    {
        Active = 0,
        Inactive = 1,
    }
}