using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Services.Interfaces;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddUserAsync(AddUserDTO dto)
    {
        await _service.AddNewUserAsync(dto);
        return Ok();
    }

    [HttpDelete("{userId:int}")]

[Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> RemoveUserAsync(int userId)
    {
        await _service.RemoveUserAync(userId);
        return Ok();
    }

}