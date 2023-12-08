using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.IpLocation;

[Table("IpLocations")]
public class IpLocationEntity : BaseEntity
{
    [Column("ip")]
    public string Ip { get; set; }
    
    [Column("city")]
    public string City { get; set; }
    
    [Column("region")]
    public string Region { get; set; }
    
    [Column("country")]
    public string Country { get; set; }

    [Column("postal")]
    public string Postal { get; set; }

    [Column("timezone")]
    public string Timezone { get; set; }

    [Column("loc")]
    public string Loc { get; set; }
}