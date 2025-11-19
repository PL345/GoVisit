using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoVisit.Core.Models;

public class Office
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("officeId")]
    public string OfficeId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("city")]
    public string City { get; set; } = string.Empty;

    [BsonElement("phone")]
    public string Phone { get; set; } = string.Empty;

    [BsonElement("services")]
    public List<string> Services { get; set; } = new();

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
}