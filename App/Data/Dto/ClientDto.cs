using System.Text.Json.Serialization;
using lanstreamer_api.App.Data.Models;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.Models;

[Serializable]
public class ClientDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("feedbacks")]
    public List<string> Feedbacks { get; set; }

    [JsonPropertyName("referrerWebsite")]
    public string? ReferrerWebsite { get; set; }

    [JsonIgnore]
    public DateTime VisitTime;

    [JsonIgnore]
    public TimeSpan TimeOnSite;

    [JsonIgnore]
    public OperatingSystem OperatingSystem;

    [JsonIgnore]
    public IpLocation? IpLocation;

    [JsonIgnore]
    public string? Language;

    [JsonIgnore]
    public int? Downloads;
}
