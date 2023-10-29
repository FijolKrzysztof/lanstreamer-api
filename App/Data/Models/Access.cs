using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;

namespace lanstreamer_api.App.Data.Models;

public class Access
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [Required]
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("timestamp")]
    public Timestamp Timestamp { get; set; }
}