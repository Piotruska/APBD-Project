using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class PayementRepository : IPayementRepository
{
    private RRConext _context;

    public PayementRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<List<Payment>> GetPayementsForContractAsync(int contract)
    {
        return await _context.Payments
            .Where(x => x.IdContract == contract)
            .ToListAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
}