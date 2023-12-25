using Newtonsoft.Json;

namespace lanstreamer_api.App.Data.Dto.Responses;

[Serializable]
public class ErrorResponse
{
    [JsonProperty("statusCode")]
    public int StatusCode { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}