using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;
using RevenueRecodnition.Api.Repositories.Interfaces;

namespace RevenueRecodnition.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly RRConext _context;

    public ProductRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }
}