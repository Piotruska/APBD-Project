using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface IDicountRepository
{
    public Task<Discount?> GetCurrentHighestDiscount();
}