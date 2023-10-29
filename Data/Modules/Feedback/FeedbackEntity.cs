using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Entities;

public class FeedbackEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("client")]
    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("message")]
    public string Message { get; set; }


    public ClientEntity Client { get; set; }
}