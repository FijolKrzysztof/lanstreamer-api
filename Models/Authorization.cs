namespace lanstreamer_api.Models;

public class Authorization
{
    public int Id { get; set; }
    public string? AuthorizationString { get; set; }
    public DateTime? Timestamp { get; set; }
}