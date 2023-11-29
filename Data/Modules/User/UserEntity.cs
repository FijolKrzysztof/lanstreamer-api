using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Modules.IpLocation;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.User;

[Table("Users")]
public class UserEntity : BaseEntity
{
    [ForeignKey("IpLocation")]
    [Column("ip_location_id")]
    public int? IpLocationId { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("google_id")]
    public string GoogleId { get; set; }

    [Column("app_version")]
    public float AppVersion { get; set; }

    [Column("last_login")]
    public DateTime LastLogin { get; set; }

    [Column("access_code")]
    public string? AccessCode { get; set; }

    public IpLocationEntity? IpLocation { get; set; }
}