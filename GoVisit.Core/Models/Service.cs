using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoVisit.Core.Models;

public class Service
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("serviceId")]
    public string ServiceId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("requiredDocuments")]
    public List<string> RequiredDocuments { get; set; } = new();

    [BsonElement("estimatedDurationMinutes")]
    public int EstimatedDurationMinutes { get; set; } = 30;

    [BsonElement("category")]
    public string Category { get; set; } = string.Empty;

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
}