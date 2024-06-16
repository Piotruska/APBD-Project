using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface IProductRepository
{
    public Task<Product?> GetrProduct(int productId);
}