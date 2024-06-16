using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface IContracrRepository
{
    public Task<Contract?> GetActiveContractForProduct(int productId,int clientId);
    public Task<List<Contract>> GetListOfSignedContracts();
    public Task<List<Contract>> GetListOfSignedContractsForProduct(int productId);
    public Task<List<Contract>> GetListOfAllContractsNotPastDates();
    public Task<List<Contract>> GetListOfAllContractsNotPastDatesForProduct(int productId);
    public Task<Contract?> GetContract(int contractId);
    public Task<int> AddContract(Contract contract);
    
    public Task SignContract(int contractId);
}