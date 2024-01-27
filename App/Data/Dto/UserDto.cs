using System.Text.Json.Serialization;
using lanstreamer_api.App.Data.Models;
using Newtonsoft.Json;

namespace lanstreamer_api.Models;

[Serializable]
public record UserDto
{
    [JsonProperty("id")]
    public int Id { get; init; }

    [JsonProperty("accessCode")]
    public string? AccessCode { get; init; }
}
