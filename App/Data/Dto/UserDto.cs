using System.ComponentModel.DataAnnotations;
using lanstreamer_api.App.Data.Models;

namespace lanstreamer_api.Models;

public class UserDto // TODO: nie wiem czy nie trzeba będzie dodać [Serializable], dodatkowo jest coś takiego jak [JsonPropertyName("name")] lub można poszukać jakiegoś automatycznego mappera UpperCamelCase na lowerCamelCase
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