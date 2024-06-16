using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface IContracrRepository
{
    public Task<Contract?> GetActiveContractForProduct(int productId,int clientId);
    public Task<Contract?> GetContract(int contractId);
    public Task<int> AddContract(Contract contract);
    
    public Task SignContract(int contractId);
}