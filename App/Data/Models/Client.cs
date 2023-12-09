using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Data.Models;

public class Client
{
    public int Id;
    public List<string>? Feedbacks;
    public string? ReferrerWebsite;
    public DateTime VisitTime;
    public TimeSpan TimeOnSite;
    public string OperatingSystem;
    public IpLocation? IpLocation;
    public string? Language;
    public int? Downloads;
}
