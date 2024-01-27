using Newtonsoft.Json;

namespace lanstreamer_api.App.Data.Dto.Responses;

[Serializable]
public record ErrorResponse
{
    [JsonProperty("statusCode")]
    public int StatusCode { get; init; }

    [JsonProperty("message")]
    public string Message { get; init; }
}