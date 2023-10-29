using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Data.Configuration;

public class ConfigurationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public string Id { get; set; }
    
    [Column("key")]
    public string Key { get; set; }
    
    [Column("value")]
    public string Value { get; set; }
}