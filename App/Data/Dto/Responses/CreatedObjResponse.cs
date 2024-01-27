using System.Text.Json.Serialization;

namespace lanstreamer_api.App.Data.Dto;

[Serializable]
public record CreatedObjResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
}
