using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace lanstreamer_api.Models;

public class Configuration
{
    [BsonId]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Key { get; set; }
    public string? Value { get; set; }
}