using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<Product?> GetProductAsync(int productId);
}