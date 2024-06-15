using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Services;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{ 
    private IClientService _service;
    
    public ClientController(IClientService service)
    {
        _service = service;
    }

    [HttpPost("/Individual")]
    public async Task<IActionResult> AddIndividualClientAsync(AddIndividualClientDTO dto)
    {
        await _service.AddIndividualClientAsync(dto);
        return NoContent();
    }
    
    [HttpPost("/Company")]
    public async Task<IActionResult> AddCompanyClientAsync(AddCompanyClientDTO dto)
    {
        await _service.AddCompanyClientAsync(dto);
        return NoContent();
    }
    
    [HttpPut("/Company/{IndividualClientId:int}")]
    public async Task<IActionResult> UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto,int IndividualClientId)
    {
        await _service.UpdateIndividualCLientAsync(dto,IndividualClientId);
        return NoContent();
    }
    
    [HttpPut("/Individual/{ConapnyClientId:int}")]
    public async Task<IActionResult> UpdateCompanyClientAsync(UpdateCompanyClientDto dto,int ConapnyClientId)
    {
        await _service.UpdateCompanyClientAsync(dto,ConapnyClientId);
        return NoContent();
    }
    
    [HttpDelete("/Individual/{IndividualClientId:int}")]
    public async Task<IActionResult> SoftDeleteIndividualCLientAsync(int IndividualClientId)
    {
        await _service.SoftDeleteIndividualCLientAsync(IndividualClientId);
        return NoContent();
    }
}