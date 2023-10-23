using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.App.Data.Models;
using lanstreamer_api.Data.Authentication;

namespace lanstreamer_api.Data.Modules.User;

[Table("User")]
public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int id { get; set; }
    
    [Column("mail")]
    public string mail { get; set; }
    
    [Column("google_id")]
    public string googleId { get; set; }
    
    [Column("app_version")]
    public int appVersion { get; set; }
    
    [Column("last_login")]
    public DateTime lastLogin { get; set; }
    
    public AccessEntity? access { get; set; }
}