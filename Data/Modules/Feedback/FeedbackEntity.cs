using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Entities;

public class FeedbackEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int id { get; set; }

    [ForeignKey("client")]
    [Column("client_id")]
    public int clientId { get; set; }

    [Column("message")] public string message { get; set; }


    public ClientEntity client { get; set; }
}