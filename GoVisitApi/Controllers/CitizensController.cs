using Microsoft.AspNetCore.Mvc;
using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitizensController : ControllerBase
{
    private readonly IRepository<Citizen> _citizenRepository;

    public CitizensController(IRepository<Citizen> citizenRepository)
    {
        _citizenRepository = citizenRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Citizen>>> GetCitizens()
    {
        var citizens = await _citizenRepository.GetAllAsync();
        return Ok(citizens.Where(c => c.IsActive));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Citizen>> GetCitizen(string id)
    {
        var citizen = await _citizenRepository.GetByIdAsync(id);
        if (citizen == null)
            return NotFound();
        
        return Ok(citizen);
    }

    [HttpGet("by-citizen-id/{citizenId}")]
    public async Task<ActionResult<Citizen>> GetCitizenByCitizenId(string citizenId)
    {
        var citizens = await _citizenRepository.GetAllAsync();
        var citizen = citizens.FirstOrDefault(c => c.CitizenId == citizenId && c.IsActive);
        
        if (citizen == null)
            return NotFound();
        
        return Ok(citizen);
    }

    [HttpPost]
    public async Task<ActionResult<Citizen>> CreateCitizen(Citizen citizen)
    {
        var createdCitizen = await _citizenRepository.CreateAsync(citizen);
        return CreatedAtAction(nameof(GetCitizen), new { id = createdCitizen.Id }, createdCitizen);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCitizen(string id, Citizen citizen)
    {
        var updated = await _citizenRepository.UpdateAsync(id, citizen);
        if (!updated)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCitizen(string id)
    {
        var deleted = await _citizenRepository.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}