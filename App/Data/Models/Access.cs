using System.ComponentModel.DataAnnotations;
using Google.Protobuf.WellKnownTypes;

namespace lanstreamer_api.App.Data.Models;

public class Access
{
    public int id { get; set; }
    
    [Required]
    public string code { get; set; }
    
    [Required]
    public Timestamp timestamp { get; set; }
}