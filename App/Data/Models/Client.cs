using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Data.Models;

public class Client
{
    public int Id;
    public DateTime VisitTime;
    public TimeSpan TimeOnSite;
    public OperatingSystem OperatingSystem;
    public IpLocation? IpLocation;
    public string? Language;
    public int? Downloads;
    public string? ReferrerWebsite;
    public List<string>? Feedbacks;
}
