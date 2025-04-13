using Microsoft.AspNetCore.Mvc;
using OrderService.Entities;
using OrderService.Services.Abstract;

namespace OrderService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var Orders = await _orderService.GetAllOrders();
        Response.Headers.Append("X-Service-Instance", Environment.GetEnvironmentVariable("SERVICE_INSTANCE") ?? "Unknown");
        return Ok(Orders);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok($"Orders endpoint working! Instance: {Environment.GetEnvironmentVariable("SERVICE_INSTANCE") ?? "Unknown"}");
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
