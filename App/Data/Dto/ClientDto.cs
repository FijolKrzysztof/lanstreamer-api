using System.ComponentModel.DataAnnotations;

namespace lanstreamer_api.Models;

public class ClientDto
{
    public int id { get; set; }
    
    [Required]
    public DateTime visitTime { set; get; }
    
    [Required]
    public TimeSpan timeOnSite { get; set; }
    
    [Required]
    public string operatingSystem { get; set; }
    
    [Required]
    public string defaultLanguage { get; set; }
    
    [Required]
    public string ipAddress { get; set; }
    
    public string? referrerPage { get; set; }
    public int? downloads { get; set; }
    public List<string>? feedbacks { get; set; }
}
