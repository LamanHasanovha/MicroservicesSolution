using ProductService.Entities;

namespace ProductService.Services.Abstract;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProducts();
}
