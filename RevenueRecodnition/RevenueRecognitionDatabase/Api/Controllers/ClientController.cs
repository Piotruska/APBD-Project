using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Services;

namespace RevenueRecodnition.Api.Controllers;

public class ClientController : ControllerBase
{ 
    private IClientService _service;
    
    public ClientController(IClientService service)
    {
        _service = service;
    }

    /// <summary>
    /// Endpoints used to .
    /// </summary>
    /// <returns>...</returns>
    /// HttpGet - Get data
    /// HttpPost - Add data
    /// HttpPut - Update Data
    /// HttpDelete - Selete Data

    [HttpPost("api/")]
    public async Task<IActionResult> GetAsync()
    {
        var a = 0;
        return Ok(a);
    }
}