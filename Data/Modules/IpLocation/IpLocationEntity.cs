using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Data.Modules.IpLocation;

[Table("IpLocations")]
public class IpLocationEntity
{
    [Key]
    [Column("id")]
    public string Ip;
    
    [Column("city")]
    public string City;
    
    [Column("region")]
    public string Region;
    
    [Column("country")]
    public string Country;
    
    [Column("loc")]
    public string Loc;
    
    [Column("org")]
    public string Org;
    
    [Column("postal")]
    public string Postal;
    
    [Column("timezone")]
    public string Timezone;
    
    [Column("readme")]
    public string Readme;
}