using Microsoft.AspNetCore.Mvc;
using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IRepository<Service> _serviceRepository;

    public ServicesController(IRepository<Service> serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        var services = await _serviceRepository.GetAllAsync();
        return Ok(services.Where(s => s.IsActive));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetService(string id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
            return NotFound();
        
        return Ok(service);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<Service>>> GetServicesByCategory(string category)
    {
        var services = await _serviceRepository.GetAllAsync();
        var filteredServices = services.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && s.IsActive);
        return Ok(filteredServices);
    }

    [HttpPost]
    public async Task<ActionResult<Service>> CreateService(Service service)
    {
        var createdService = await _serviceRepository.CreateAsync(service);
        return CreatedAtAction(nameof(GetService), new { id = createdService.Id }, createdService);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(string id, Service service)
    {
        var updated = await _serviceRepository.UpdateAsync(id, service);
        if (!updated)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string id)
    {
        var deleted = await _serviceRepository.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}