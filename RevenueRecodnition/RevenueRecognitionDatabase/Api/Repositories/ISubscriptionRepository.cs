using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface ISubscriptionRepository
{
    public Task<Subscription?> GetActiveSubscriptionForProduct(int productId,int clientId);
}