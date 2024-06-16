using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services;

public interface IContractService
{
    public Task<int> CreateContractAsync(CreateContractDTO dto);
    public Task IssuePayementForContractAsync(PaymentForContractDTO dto);
}