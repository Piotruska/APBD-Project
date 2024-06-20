using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services.Interfaces;

public interface ISubscriptionService
{
    public Task<int> AddSubscriptionAsync(AddSubscriptionDTO dto);
    public Task PayForSubscriptionAsync(PayementForSubscription dto);
}