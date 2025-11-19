using System.ComponentModel.DataAnnotations;

namespace GoVisit.Core.Models;

public class CreateAppointmentRequest
{
    [Required]
    public string CitizenId { get; set; } = string.Empty;

    [Required]
    public string CitizenName { get; set; } = string.Empty;

    [Required]
    public string CitizenPhone { get; set; } = string.Empty;

    [Required]
    public string OfficeId { get; set; } = string.Empty;

    [Required]
    public string ServiceType { get; set; } = string.Empty;

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public TimeSpan AppointmentTime { get; set; }
}