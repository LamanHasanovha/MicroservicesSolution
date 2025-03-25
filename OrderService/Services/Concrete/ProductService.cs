using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess;
using OrderService.Entities;
using OrderService.Services.Abstract;

namespace OrderService.Services.Concrete;

public class OrderService : IOrderService
{
    private readonly LamanDbContext _context;

    public OrderService(LamanDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _context.Orders.ToListAsync();
    }
}
