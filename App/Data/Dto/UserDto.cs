using System.Text.Json.Serialization;
using lanstreamer_api.App.Data.Models;

namespace lanstreamer_api.Models;

[Serializable]
public class UserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("accessCode")]
    public string? AccessCode { get; set; }

    [JsonIgnore]
    public string Email { get; set; }

    [JsonIgnore]
    public string GoogleId { get; set; }

    [JsonIgnore]
    public float AppVersion { get; set; }

    [JsonIgnore]
    public DateTime LastLogin { get; set; }

    [JsonIgnore]
    public IpLocation? IpLocation { get; set; }
}
