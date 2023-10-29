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
    public int Id { get; set; }
    
    [Column("mail")]
    public string Mail { get; set; }
    
    [Column("google_id")]
    public string GoogleId { get; set; }
    
    [Column("app_version")]
    public int AppVersion { get; set; }
    
    [Column("last_login")]
    public DateTime LastLogin { get; set; }
    
    public AccessEntity? Access { get; set; }
}