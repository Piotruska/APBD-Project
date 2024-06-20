using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    public Task AddSubscriptionAsync(Subscription subscription);
    
    public Task<Subscription?> GetSubscriptionAsync(int subscriptionId);
    
    public Task<Subscription?> GetActiveSubscriptionsForProductAsync(int productId,int clientId);
    public Task<List<Subscription>> GetListOfSubscriptionsWithPayementsAsync();
    public Task<List<Subscription>> GetListOfSubscriptionsWithPayementsForProductAsync(int productId);
    public Task<List<Subscription>> GetListOfSubscriptionsNotCanceledAsync();

    public Task<List<Subscription>> GetListOfSubscriptionsForProductNotCanceledAsync(int productId);
    
    public Task UpdateSubscriptionDatesAsync(int subscriptionId);
}