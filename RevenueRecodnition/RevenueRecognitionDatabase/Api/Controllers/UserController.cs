using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.MiddleWares;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Services;


namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }


    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllData(AddUserDTO dto)
    {
        await _service.AddNewUserAsync(dto);
        return Ok();
    }
}