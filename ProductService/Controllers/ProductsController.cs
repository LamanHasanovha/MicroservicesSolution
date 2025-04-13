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
        Response.Headers.Append("X-Service-Instance", Environment.GetEnvironmentVariable("SERVICE_INSTANCE") ?? "Unknown");
        return Ok(products);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok($"Products endpoint working! Instance: {Environment.GetEnvironmentVariable("SERVICE_INSTANCE") ?? "Unknown"}");
    }

    [HttpGet("instance-info")]
    public IActionResult GetInstanceInfo()
    {
        return Ok(new
        {
            Instance = Environment.GetEnvironmentVariable("SERVICE_INSTANCE") ?? "Unknown",
            Timestamp = DateTime.UtcNow
        });
    }
}
