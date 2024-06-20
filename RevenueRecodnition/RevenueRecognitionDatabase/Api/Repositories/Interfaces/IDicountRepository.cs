using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface IDicountRepository
{
    public Task<Discount?> GetCurrentHighestDiscountAsync();
}