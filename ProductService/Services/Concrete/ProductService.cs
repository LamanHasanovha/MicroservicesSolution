using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess;
using ProductService.Entities;
using ProductService.Services.Abstract;

namespace ProductService.Services.Concrete;

public class ProductService : IProductService
{
    private readonly LamanDbContext _context;

    public ProductService(LamanDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }
}
