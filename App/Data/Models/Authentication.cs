using System.ComponentModel.DataAnnotations;
using Google.Protobuf.WellKnownTypes;

namespace lanstreamer_api.App.Data.Models;

public class Authentication
{
    public int id { get; set; }
    
    [Required]
    public string accessCode { get; set; }
    
    [Required]
    public Timestamp timestamp { get; set; }
}