using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface IPayementRepository
{
    public Task<List<Payment>> GetPayementsForContractAsync(int contract);
    
    public Task AddPaymentAsync(Payment payment);
}