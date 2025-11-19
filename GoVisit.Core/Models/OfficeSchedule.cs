using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoVisit.Core.Models;

public class OfficeSchedule
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("officeId")]
    public string OfficeId { get; set; } = string.Empty;

    [BsonElement("serviceType")]
    public string ServiceType { get; set; } = string.Empty;

    [BsonElement("weeklySchedule")]
    public Dictionary<string, DaySchedule> WeeklySchedule { get; set; } = new();

    [BsonElement("dateOverrides")]
    public List<DateOverride> DateOverrides { get; set; } = new();
}

public class DaySchedule
{
    [BsonElement("isOpen")]
    public bool IsOpen { get; set; } = true;

    [BsonElement("startTime")]
    public TimeSpan StartTime { get; set; }

    [BsonElement("endTime")]
    public TimeSpan EndTime { get; set; }

    [BsonElement("slotDurationMinutes")]
    public int SlotDurationMinutes { get; set; } = 30;

    [BsonElement("breakStart")]
    public TimeSpan? BreakStart { get; set; }

    [BsonElement("breakEnd")]
    public TimeSpan? BreakEnd { get; set; }
}

public class DateOverride
{
    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("isClosed")]
    public bool IsClosed { get; set; } = false;

    [BsonElement("customSchedule")]
    public DaySchedule? CustomSchedule { get; set; }

    [BsonElement("reason")]
    public string? Reason { get; set; }
}