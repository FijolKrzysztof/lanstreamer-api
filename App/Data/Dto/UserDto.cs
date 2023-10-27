using System.ComponentModel.DataAnnotations;
using lanstreamer_api.App.Data.Models;

namespace lanstreamer_api.Models;

public class UserDto
{
    public int id { get; set; }
    
    [Required]
    public string mail { get; set; }
    
    [Required]
    public string googleId { get; set; }
    
    public float appVersion { get; set; }
    public DateTime lastLogin { get; set; }

    public Access? access { get; set; }
}