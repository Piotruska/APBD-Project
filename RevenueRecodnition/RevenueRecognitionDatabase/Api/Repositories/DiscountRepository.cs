using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class DiscountRepository : IDicountRepository
{
    private RRConext _context;

    public DiscountRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Discount?> GetCurrentHighestDiscount()
    {
        return await _context.Discounts
            .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
            .OrderBy(x => x.Percentage)
            .FirstOrDefaultAsync();
    }
}