using RevenueRecodnition.DataBase.Context;

namespace RevenueRecodnition.Api.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RRConext _context;

    public UnitOfWork(RRConext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}