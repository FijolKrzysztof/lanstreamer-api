using System.Text.Json.Serialization;

namespace lanstreamer_api.Models.Responses;

[Serializable]
public class LoginResponseDto
{
    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; }
}