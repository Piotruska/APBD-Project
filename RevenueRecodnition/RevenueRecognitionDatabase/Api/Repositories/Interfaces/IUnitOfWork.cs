namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface IUnitOfWork
{
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}