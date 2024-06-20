using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface IContracrRepository
{
    public Task<Contract?> GetActiveContractForProductAsync(int productId,int clientId);
    public Task<List<Contract>> GetListOfSignedContractsAsync();
    public Task<List<Contract>> GetListOfSignedContractsForProductAsync(int productId);
    public Task<List<Contract>> GetListOfAllContractsNotPastDatesAsync();
    public Task<List<Contract>> GetListOfAllContractsNotPastDatesForProductAsync(int productId);
    public Task<Contract?> GetContractAsync(int contractId);
    public Task<int> AddContractAsync(Contract contract);
    
    public Task SignContractAsync(int contractId);
}