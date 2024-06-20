using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;
using RevenueRecodnition.Api.Repositories.Interfaces;

namespace RevenueRecodnition.Api.Repositories;

public class DiscountRepository : IDicountRepository
{
    private RRConext _context;

    public DiscountRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Discount?> GetCurrentHighestDiscountAsync()
    {
        return await _context.Discounts
            .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
            .OrderByDescending(x => x.Percentage)
            .FirstOrDefaultAsync();
    }
}