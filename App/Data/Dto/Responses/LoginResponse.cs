using System.Text.Json.Serialization;

namespace lanstreamer_api.Models.Responses;

[Serializable]
public record LoginResponse
{
    [JsonPropertyName("roles")]
    public List<string> Roles { get; init; }
}