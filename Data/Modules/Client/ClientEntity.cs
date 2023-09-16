using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Entities;

[Table("Client")]
public class ClientEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int id { get; set; }
    
    [Column("visit_time")]
    public DateTime visitTime { set; get; }
    
    [Column("operating_system")]
    public string operatingSystem { get; set; }
    
    [Column("default_language")]
    public string defaultLanguage { get; set; }
    
    [Column("time_on_site")]
    public TimeSpan timeOnSite { get; set; }
    
    [Column("ip_address")]
    public string ipAddress { get; set; }
    
    [Column("referrer_page")]
    public string? referrerPage { get; set; }
    
    [Column("downloads")]
    public int? downloads { get; set; }
    
    
    public List<FeedbackEntity> feedbacks { get; set; }
}