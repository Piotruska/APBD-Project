using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Services;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/revenue")]
[Authorize]
public class RevenueController : ControllerBase
{
    private IRevenueService _service;

    public RevenueController(IRevenueService service)
    {
        _service = service;
    }
    
    
    [HttpPost("CalculateRevenue")]
    public async Task<IActionResult> GetCurrentReveniueAsync(RevenueCalculationRequestDTO requestDto)
    {
        var amount = await _service.CalculateCurrentRevenueAsync(requestDto);
        return Ok(amount + " "+ requestDto.CurrencyCode);
    }
    
    
    [HttpPost("CalculatePrecictedRevenue")]
    public async Task<IActionResult> GetPredictedRevenueAsync(RevenueCalculationRequestDTO requestDto)
    {
        var amount = await _service.CalculatePredictedRevenueAsync(requestDto);
        return Ok(amount + " "+ requestDto.CurrencyCode);
    }
}