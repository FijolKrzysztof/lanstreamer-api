using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Modules.User;

namespace lanstreamer_api.Data.Authentication;

public class AccessEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int id { get; set; }
    
    [ForeignKey("user")]
    [Column("user_id")]
    public int userId { get; set; }
    
    [Column("code")]
    public string code { get; set; }
    
    [Column("timestamp")]
    public DateTime timestamp { get; set; }
    
    
    public UserEntity user { get; set; }
}