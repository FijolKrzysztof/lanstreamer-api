using System.Text.Json.Serialization;
using lanstreamer_api.App.Data.Models;
using Newtonsoft.Json;

namespace lanstreamer_api.Models;

[Serializable]
public class UserDto
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("accessCode")]
    public string? AccessCode { get; set; }
}
