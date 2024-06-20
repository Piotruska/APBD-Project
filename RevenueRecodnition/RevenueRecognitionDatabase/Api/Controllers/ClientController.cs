using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Services.Interfaces;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientController : ControllerBase
{ 
    private readonly IClientService _service;
    
    public ClientController(IClientService service)
    {
        _service = service;
    }

    
    [HttpPost("individualClients")]
    public async Task<IActionResult> AddIndividualClientAsync(AddIndividualClientDTO dto)
    {
        await _service.AddIndividualClientAsync(dto);
        return NoContent();
    }
    
    
    [HttpPost("companyClients")]
    public async Task<IActionResult> AddCompanyClientAsync(AddCompanyClientDTO dto)
    {
        await _service.AddCompanyClientAsync(dto);
        return NoContent();
    }
    
    [HttpPut("companyClients/{CompanyClientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateCompanyClientAsync(UpdateCompanyClientDto dto,int CompanyClientId)
    {
        await _service.UpdateCompanyClientAsync(dto,CompanyClientId);
        return NoContent();
    }
    
    [HttpPut("individualClients/{IndividualClientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto,int IndividualClientId)
    {
        await _service.UpdateIndividualCLientAsync(dto,IndividualClientId);
        return NoContent();
    }
    
    [HttpDelete("individualClients/{IndividualClientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> SoftDeleteIndividualCLientAsync(int IndividualClientId)
    {
        await _service.SoftDeleteIndividualCLientAsync(IndividualClientId);
        return NoContent();
    }
}