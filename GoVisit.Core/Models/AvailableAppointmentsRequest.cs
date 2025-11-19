using System.ComponentModel.DataAnnotations;

namespace GoVisit.Core.Models;

public class AvailableAppointmentsRequest
{
    [Required]
    public string ServiceType { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? OfficeId { get; set; }
}
