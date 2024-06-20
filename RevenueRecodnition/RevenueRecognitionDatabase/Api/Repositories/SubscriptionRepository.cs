using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;
using RevenueRecodnition.Api.Repositories.Interfaces;

namespace RevenueRecodnition.Api.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private RRConext _context;

    public SubscriptionRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetSubscriptionAsync(int subscriptionId)
    {
        return await _context.Subscriptions.FindAsync(subscriptionId);
    }

    public async Task<Subscription?> GetActiveSubscriptionsForProductAsync(int productId,int clientId)
    {
        return await _context.Subscriptions
            .Where(x => x.IdProduct == productId && x.IdClient == clientId)
            .Where(x => !x.Canceled)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Subscription>> GetListOfSubscriptionsWithPayementsAsync()
    {
        return await _context.Subscriptions
            .Include(x => x.Payments)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetListOfSubscriptionsWithPayementsForProductAsync(int productId)
    {
        return await _context.Subscriptions
            .Include(x => x.Payments)
            .Where(x=>x.IdProduct == productId)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetListOfSubscriptionsNotCanceledAsync()
    {
        return await _context.Subscriptions
            .Where(x => !x.Canceled)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetListOfSubscriptionsForProductNotCanceledAsync(int productId)
    {
        return await _context.Subscriptions
            .Where(x=>x.IdProduct == productId)
            .Where(x=>!x.Canceled)
            .ToListAsync();
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubscriptionDatesAsync(int subscriptionId)
    {
        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);

        subscription.StartDateRenewalPayement = subscription.EndDateRenewalPayement;
        subscription.EndDateRenewalPayement =
            subscription.StartDateRenewalPayement.AddMonths(subscription.RenewalPeriod);

        await _context.SaveChangesAsync();
    }
}