namespace GoVisit.Core.Models;

public class AvailableAppointmentSlot
{
    public string OfficeId { get; set; } = string.Empty;
    public string OfficeName { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int DurationMinutes { get; set; }
}
