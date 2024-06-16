using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private RRConext _context;

    public SubscriptionRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetActiveSubscriptionForProductAsync(int productId,int clientId)
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
}