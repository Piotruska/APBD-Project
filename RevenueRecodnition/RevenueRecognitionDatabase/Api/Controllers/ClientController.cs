using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Services;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientController : ControllerBase
{ 
    private IClientService _service;
    
    public ClientController(IClientService service)
    {
        _service = service;
    }

    
    [HttpPost("Individual")]
    public async Task<IActionResult> AddIndividualClientAsync(AddIndividualClientDTO dto)
    {
        await _service.AddIndividualClientAsync(dto);
        return NoContent();
    }
    
    
    [HttpPost("Company")]
    public async Task<IActionResult> AddCompanyClientAsync(AddCompanyClientDTO dto)
    {
        await _service.AddCompanyClientAsync(dto);
        return NoContent();
    }
    
    [HttpPut("Company/{CompanyClientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateCompanyClientAsync(UpdateCompanyClientDto dto,int CompanyClientId)
    {
        await _service.UpdateCompanyClientAsync(dto,CompanyClientId);
        return NoContent();
    }
    
    [HttpPut("Individual/{IndividualClientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto,int IndividualClientId)
    {
        await _service.UpdateIndividualCLientAsync(dto,IndividualClientId);
        return NoContent();
    }
    
    [HttpDelete("Individual/{IndividualClientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> SoftDeleteIndividualCLientAsync(int IndividualClientId)
    {
        await _service.SoftDeleteIndividualCLientAsync(IndividualClientId);
        return NoContent();
    }
}