using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Services;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/revenue")]
public class RevenueController : ControllerBase
{
    private IRevenueService _service;

    public RevenueController(IRevenueService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentReveniueAsync()
    {
        
        return NoContent();
    }
    
    [HttpGet("/predicted")]
    public async Task<IActionResult> GetPredictedRevenueAsync()
    {
        
        return NoContent();
    }
}