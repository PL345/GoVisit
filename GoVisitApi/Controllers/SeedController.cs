using Microsoft.AspNetCore.Mvc;
using GoVisit.Application.Services;

namespace GoVisitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly DataSeederService _seederService;

    public SeedController(DataSeederService seederService)
    {
        _seederService = seederService;
    }

    [HttpPost("seed-data")]
    public async Task<IActionResult> SeedData()
    {
        try
        {
            await _seederService.SeedDataAsync();
            return Ok(new { message = "נתונים נוספו בהצלחה למערכת", success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "שגיאה בהוספת נתונים", error = ex.Message, success = false });
        }
    }
}