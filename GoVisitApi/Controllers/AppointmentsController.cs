using Microsoft.AspNetCore.Mvc;
using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Appointment>>>> GetAppointments()
    {
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        return Ok(ApiResponse<IEnumerable<Appointment>>.SuccessResult(appointments, "התורים נטענו בהצלחה"));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Appointment>>> GetAppointment(string id)
    {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appointment == null)
            return NotFound(ApiResponse<Appointment>.ErrorResult("התור לא נמצא", "NOT_FOUND"));
        
        return Ok(ApiResponse<Appointment>.SuccessResult(appointment, "התור נמצא בהצלחה"));
    }

    [HttpGet("office/{officeId}")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByOffice(string officeId)
    {
        var appointments = await _appointmentService.GetAppointmentsByOfficeAsync(officeId);
        return Ok(appointments);
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByDate(DateTime date)
    {
        var appointments = await _appointmentService.GetAppointmentsByDateAsync(date);
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Appointment>>> CreateAppointment(CreateAppointmentRequest request)
    {
        var appointment = await _appointmentService.CreateAppointmentAsync(request);
        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, 
            ApiResponse<Appointment>.SuccessResult(appointment, "התור נקבע בהצלחה"));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateAppointmentStatus(string id, [FromBody] AppointmentStatus status)
    {
        var updated = await _appointmentService.UpdateAppointmentStatusAsync(id, status);
        if (!updated)
            return NotFound();

        return NoContent();
    }
}