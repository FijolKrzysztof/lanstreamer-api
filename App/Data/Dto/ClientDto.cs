using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace lanstreamer_api.Models;

[Serializable]
public class ClientDto
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("referrerWebsite")]
    public string? ReferrerWebsite { get; set; }
}
