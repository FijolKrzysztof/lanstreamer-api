using Newtonsoft.Json;

namespace lanstreamer_api.Models;

[Serializable]
public record ClientDto
{
    [JsonProperty("id")]
    public int Id { get; init; }

    [JsonProperty("referrerWebsite")]
    public string? ReferrerWebsite { get; init; }
}
