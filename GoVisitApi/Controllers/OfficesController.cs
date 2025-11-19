using Microsoft.AspNetCore.Mvc;
using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfficesController : ControllerBase
{
    private readonly IOfficeService _officeService;

    public OfficesController(IOfficeService officeService)
    {
        _officeService = officeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Office>>> GetOffices()
    {
        var offices = await _officeService.GetAllOfficesAsync();
        return Ok(offices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Office>> GetOffice(string id)
    {
        var office = await _officeService.GetOfficeByIdAsync(id);
        if (office == null)
            return NotFound();
        
        return Ok(office);
    }

    [HttpGet("by-service/{serviceType}")]
    public async Task<ActionResult<IEnumerable<Office>>> GetOfficesByService(string serviceType)
    {
        var offices = await _officeService.GetOfficesByServiceAsync(serviceType);
        return Ok(offices);
    }

    [HttpPost]
    public async Task<ActionResult<Office>> CreateOffice(Office office)
    {
        var createdOffice = await _officeService.CreateOfficeAsync(office);
        return CreatedAtAction(nameof(GetOffice), new { id = createdOffice.Id }, createdOffice);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOffice(string id, Office office)
    {
        var updated = await _officeService.UpdateOfficeAsync(id, office);
        if (!updated)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOffice(string id)
    {
        var deleted = await _officeService.DeleteOfficeAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}