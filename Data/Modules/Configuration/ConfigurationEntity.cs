using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Configuration;

[Table("Configurations")]
public class ConfigurationEntity : BaseEntity
{
    [Column("key")]
    public string Key { get; set; }
    
    [Column("value")]
    public string Value { get; set; }
}