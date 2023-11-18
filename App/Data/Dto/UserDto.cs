using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using lanstreamer_api.App.Data.Models;

namespace lanstreamer_api.Models;

[Serializable]
public class UserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [Required]
    [JsonPropertyName("mail")]
    public string Mail { get; set; }
    
    [Required]
    [JsonPropertyName("googleId")]
    public string GoogleId { get; set; }
    
    [JsonPropertyName("appVersion")]
    public float AppVersion { get; set; }
    
    [JsonPropertyName("lastLogin")]
    public DateTime LastLogin { get; set; }

    [JsonPropertyName("access")]
    public Access? Access { get; set; }
    
    [JsonIgnore]
    public IpLocation? IpLocation { get; set; }
}