using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Modules.IpLocation;
using lanstreamer_api.Data.Utils;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.Entities;

[Table("Clients")]
public class ClientEntity : BaseEntity
{
    [ForeignKey("IpLocation")]
    [Column("ip_location_id")]
    public int? IpLocationId { get; set; }
    
    [Column("visit_time")]
    public DateTime VisitTime { set; get; }
    
    [Column("operating_system")]
    public string OperatingSystem { get; set; }
    
    [Column("default_language")]
    public string Language { get; set; }
    
    [Column("time_on_site")]
    public TimeSpan TimeOnSite { get; set; }
    
    [Column("referrer_website")]
    public string? ReferrerWebsite { get; set; }
    
    [Column("downloads")]
    public int? Downloads { get; set; }
    
    
    public List<FeedbackEntity> Feedbacks { get; set; }
    public IpLocationEntity? IpLocation { get; set; }
}