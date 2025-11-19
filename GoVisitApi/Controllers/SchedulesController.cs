using Microsoft.AspNetCore.Mvc;
using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IRepository<OfficeSchedule> _scheduleRepository;

    public SchedulesController(IRepository<OfficeSchedule> scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OfficeSchedule>>> GetSchedules()
    {
        var schedules = await _scheduleRepository.GetAllAsync();
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OfficeSchedule>> GetSchedule(string id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null)
            return NotFound();
        
        return Ok(schedule);
    }

    [HttpGet("office/{officeId}/service/{serviceType}")]
    public async Task<ActionResult<OfficeSchedule>> GetScheduleByOfficeAndService(string officeId, string serviceType)
    {
        var schedules = await _scheduleRepository.GetAllAsync();
        var schedule = schedules.FirstOrDefault(s => s.OfficeId == officeId && s.ServiceType == serviceType);
        
        if (schedule == null)
            return NotFound();
        
        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<OfficeSchedule>> CreateSchedule(OfficeSchedule schedule)
    {
        var createdSchedule = await _scheduleRepository.CreateAsync(schedule);
        return CreatedAtAction(nameof(GetSchedule), new { id = createdSchedule.Id }, createdSchedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(string id, OfficeSchedule schedule)
    {
        var updated = await _scheduleRepository.UpdateAsync(id, schedule);
        if (!updated)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(string id)
    {
        var deleted = await _scheduleRepository.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}