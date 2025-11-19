using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoVisit.Core.Models;

public class Appointment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("citizenId")]
    public string CitizenId { get; set; } = string.Empty;

    [BsonElement("citizenName")]
    public string CitizenName { get; set; } = string.Empty;

    [BsonElement("citizenPhone")]
    public string CitizenPhone { get; set; } = string.Empty;

    [BsonElement("officeId")]
    public string OfficeId { get; set; } = string.Empty;

    [BsonElement("serviceType")]
    public string ServiceType { get; set; } = string.Empty;

    [BsonElement("appointmentDate")]
    public DateTime AppointmentDate { get; set; }

    [BsonElement("appointmentTime")]
    public TimeSpan AppointmentTime { get; set; }

    [BsonElement("status")]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum AppointmentStatus
{
    Scheduled,
    Confirmed,
    Completed,
    Cancelled
}