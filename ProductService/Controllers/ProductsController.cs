using Microsoft.AspNetCore.Mvc;
using ProductService.Entities;
using ProductService.Services.Abstract;

namespace ProductService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Products endpoint working!");
    }
}
