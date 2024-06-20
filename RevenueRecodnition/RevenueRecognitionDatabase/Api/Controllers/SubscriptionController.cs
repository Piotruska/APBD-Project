using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Services.Interfaces;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/subscriptions")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSubscriptionAsync( AddSubscriptionDTO dto)
    {
        var id = await _subscriptionService.AddSubscriptionAsync(dto);
        return Ok($"Subscription with id {id} added");
    }

    [HttpPost("payements")]

    public async Task<IActionResult> PayForSubscriptionAsync(PayementForSubscription dto)
    {
        await _subscriptionService.PayForSubscriptionAsync(dto);
        return NoContent();
    }

}