using System.Text.Json.Serialization;

namespace lanstreamer_api.Models;

[Serializable]
public class ClientDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("referrerWebsite")]
    public string? ReferrerWebsite { get; set; }

    [JsonPropertyName("feedbacks")]
    public List<string>? Feedbacks { get; set; }
}
