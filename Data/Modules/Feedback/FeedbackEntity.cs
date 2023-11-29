using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Entities;

[Table("Feedbacks")]
public class FeedbackEntity : BaseEntity
{
    [ForeignKey("Client")]
    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("message")]
    public string Message { get; set; }


    public ClientEntity Client { get; set; }
}