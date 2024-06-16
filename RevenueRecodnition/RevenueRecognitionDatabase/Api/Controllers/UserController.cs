using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.MiddleWares;


namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    [HttpGet("anonymous")]
    //[Authorize("", true)] // Allow anonymous access
    public IActionResult AnonymousEndpoint()
    {
        return Ok("This is an anonymous endpoint.");
    }

    [HttpGet("normal")]
    //[Authorize("Normal")] // Allow access to normal users
    public IActionResult NormalUserEndpoint()
    {
        return Ok("This endpoint is accessible to normal users.");
    }

    [HttpGet("admin")]
    //[Authorize("Admin")] // Allow access to admins
    public IActionResult AdminEndpoint()
    {
        return Ok("This endpoint is accessible to admins.");
    }
}