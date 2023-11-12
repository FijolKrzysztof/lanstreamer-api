using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Modules.User;

namespace lanstreamer_api.Data.Authentication;

[Table("Accesses")]
public class AccessEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("code")]
    public string Code { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamp { get; set; }
}