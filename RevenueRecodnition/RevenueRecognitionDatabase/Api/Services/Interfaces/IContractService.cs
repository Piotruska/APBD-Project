using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services.Interfaces;

public interface IContractService
{
    public Task<int> CreateContractAsync(CreateContractDTO dto);
    public Task IssuePayementForContractAsync(PaymentForContractDTO dto);
}