using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lanstreamer_api.Models;

[Serializable]
public class DownloadResponse
{
    [Required]
    [JsonPropertyName("link")]
    public string Link;
}