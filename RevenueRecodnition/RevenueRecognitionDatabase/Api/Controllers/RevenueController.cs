using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
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

    [HttpPost("/CalculateRevenue")]

    public async Task<IActionResult> GetCurrentReveniueAsync(RevenueCalculationRequest request)
    {
        var amount = await _service.CalculateCurrentRevenueAsync(request);
        return Ok(amount + " "+ request.CurrencyCode);
    }
    
    [HttpPost("/CalculatePrecictedRevenue")]
    public async Task<IActionResult> GetPredictedRevenueAsync(RevenueCalculationRequest request)
    {
        var amount = await _service.CalculatePredictedRevenueAsync(request);
        return Ok(amount + " "+ request.CurrencyCode);
    }
}