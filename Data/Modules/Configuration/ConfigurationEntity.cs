using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Data.Configuration;

public class ConfigurationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public string id { get; set; }
    
    [Column("key")]
    public string key { get; set; }
    
    [Column("value")]
    public string value { get; set; }
}