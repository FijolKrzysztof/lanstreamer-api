using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.AccessCode;

public class AccessEntity : BaseEntity
{
    [ForeignKey("User")]
    [Column("user_id")]
    public int? UserId { get; set; }
    
    [Column("code")]
    public string Code { get; set; }
    
    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }

    public UserEntity? User { get; set; }
}