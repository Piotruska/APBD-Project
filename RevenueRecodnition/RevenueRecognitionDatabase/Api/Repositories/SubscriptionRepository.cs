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

    public async Task<Subscription?> GetActiveSubscriptionForProduct(int productId,int clientId)
    {
        return await _context.Subscriptions
            .Where(x => x.IdProduct == productId && x.IdClient == clientId)
            .Where(x => !x.Canceled)
            .FirstOrDefaultAsync();
    }
}