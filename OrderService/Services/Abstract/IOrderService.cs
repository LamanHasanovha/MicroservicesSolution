using OrderService.Entities;

namespace OrderService.Services.Abstract;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrders();
}
