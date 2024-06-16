using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface ISubscriptionRepository
{
    public Task<Subscription?> GetActiveSubscriptionForProductAsync(int productId,int clientId);
    public Task<List<Subscription>> GetListOfSubscriptionsWithPayementsAsync();
    public Task<List<Subscription>> GetListOfSubscriptionsWithPayementsForProductAsync(int productId);
    public Task<List<Subscription>> GetListOfSubscriptionsNotCanceledAsync();

    public Task<List<Subscription>> GetListOfSubscriptionsForProductNotCanceledAsync(int productId);
}