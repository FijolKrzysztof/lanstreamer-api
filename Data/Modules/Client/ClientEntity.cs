using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Entities;

[Table("Client")]
public class ClientEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("visit_time")]
    public DateTime VisitTime { set; get; }
    
    [Column("operating_system")]
    public string OperatingSystem { get; set; }
    
    [Column("default_language")]
    public string DefaultLanguage { get; set; }
    
    [Column("time_on_site")]
    public TimeSpan TimeOnSite { get; set; }
    
    [Column("ip_address")]
    public string IpAddress { get; set; }
    
    [Column("referrer_page")]
    public string? ReferrerPage { get; set; }
    
    [Column("downloads")]
    public int? Downloads { get; set; }
    
    
    public List<FeedbackEntity> Feedbacks { get; set; }
}