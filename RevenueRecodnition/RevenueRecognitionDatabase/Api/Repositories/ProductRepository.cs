using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private RRConext _context;

    public ProductRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Product?> GetrProduct(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }
}