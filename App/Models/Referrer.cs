using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace lanstreamer_api.Models;

public class Referrer
{
    [BsonId]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime? Timestamp { get; set; }
}