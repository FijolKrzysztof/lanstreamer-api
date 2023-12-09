using System.Text.Json.Serialization;

namespace lanstreamer_api.App.Data.Dto;

[Serializable]
public class CreatedObjResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
