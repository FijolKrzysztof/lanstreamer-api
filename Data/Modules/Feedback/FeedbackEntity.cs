using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanstreamer_api.Entities;

[Table("Feedbacks")]
public class FeedbackEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Client")]
    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("message")]
    public string Message { get; set; }


    public ClientEntity Client { get; set; }
}