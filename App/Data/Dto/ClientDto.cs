using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lanstreamer_api.Models;

[Serializable]
public class ClientDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [Required]
    [JsonPropertyName("visitTime")]
    public DateTime VisitTime { set; get; }
    
    [Required]
    [JsonPropertyName("timeOnSite")]
    public TimeSpan TimeOnSite { get; set; }
    
    [Required]
    [JsonPropertyName("operatingSystem")]
    public string OperatingSystem { get; set; }
    
    [Required]
    [JsonPropertyName("defaultLanguage")]
    public string DefaultLanguage { get; set; }
    
    [Required]
    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; }
    
    [JsonPropertyName("referrerWebsite")]
    public string? ReferrerWebsite { get; set; }
    
    [JsonPropertyName("downloads")]
    public int? Downloads { get; set; }
    
    [JsonPropertyName("feedbacks")]
    public List<string>? Feedbacks { get; set; }
}
