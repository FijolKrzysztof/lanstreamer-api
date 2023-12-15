namespace lanstreamer_api.App.Data.Models;

public class User
{
    public int Id { get; set; }
    public string? AccessCode { get; set; }
    public string Email { get; set; }
    public string GoogleId { get; set; }
    public float AppVersion { get; set; }
    public DateTime LastLogin { get; set; }
    public IpLocation? IpLocation { get; set; }
}