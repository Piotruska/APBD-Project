using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Services.Interfaces;

namespace RevenueRecodnition.Api.Controllers;

[ApiController]
[Route("api/contracts")]
[Authorize]
public class ContractController : ControllerBase
{
    private readonly IContractService _service;

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
    
    
    [HttpPost("payements")]
    public async Task<IActionResult> IssuePayementForContractAsync(PaymentForContractDTO dto)
    {
        await _service.IssuePayementForContractAsync(dto);
        return NoContent();
    }

    
    
    
    
    
}