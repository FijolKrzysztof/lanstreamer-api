using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace lanstreamer_api.Models;

public class Authorization
{
    [BsonId]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? AuthorizationString { get; set; }
    public DateTime? Timestamp { get; set; }
}