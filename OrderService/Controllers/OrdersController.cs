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
        return Ok(Orders);
    }
}
