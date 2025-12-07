namespace Servers;

using Entitys;
using Repository;


public class OrdersService : IOrdersService
{
    private readonly IOrderRepository _orderRepository;

    public OrdersService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await _orderRepository.GetOrderById(id);
    }

    public async Task<Order> AddOrder(Order order)
    {

        return await _orderRepository.AddOrder(order);

    }
}
