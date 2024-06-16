using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Services;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/contract")]
[Authorize]
public class ContractController : ControllerBase
{
    private IContractService _service;

    public ContractController(IContractService service)
    {
        _service = service;
    }

    
    [HttpPost("")]
    public async Task<IActionResult> CreateContractAsync(CreateContractDTO dto)
    {
        var contractId = await _service.CreateContractAsync(dto);
        return Ok($"Contract created with Id : {contractId}");
    }
    
    
    [HttpPost("Payement")]
    public async Task<IActionResult> IssuePayementForContractAsync(PaymentForContractDTO dto)
    {
        await _service.IssuePayementForContractAsync(dto);
        return NoContent();
    }

    
    
    
    
    
}