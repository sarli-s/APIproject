namespace Servers;

using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Logging;
using Repositories;

public class OrdersService : IOrdersService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrdersService> _logger;
    private readonly IKafkaProducerService _kafkaProducer;

    public OrdersService(
        IOrderRepository orderRepository,
        IMapper mapper,
        IProductRepository productRepository,
        ILogger<OrdersService> logger,
        IKafkaProducerService kafkaProducer)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _logger = logger;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<OrderDTO> GetOrderById(int id)
    {
        return _mapper.Map<OrderDTO>(await _orderRepository.GetOrderById(id));
    }

    public async Task<IEnumerable<OrderDTO>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllOrders();
        return _mapper.Map<IEnumerable<OrderDTO>>(orders);
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersByUserId(int userId)
    {
        var orders = await _orderRepository.GetAllOrders();
        return _mapper.Map<IEnumerable<OrderDTO>>(orders.Where(o => o.UserId == userId));
    }

    public async Task<OrderDTO> AddOrder(OrderDTO order)
    {
        // Server-side price recalculation — never trust client sum
        double realSum = 0;
        foreach (OrderItemDTO item in order.OrderItems)
        {
            Product product = await _productRepository.GetProductById(item.ProductId);
            if (product != null)
                realSum += product.Price * item.Quantity;
        }

        if (realSum != order.OrderSum)
        {
            _logger.LogWarning("Order sum mismatch for user {UserId}. Client: {Client}, Server: {Server}",
                order.userId, order.OrderSum, realSum);
            order = order with { OrderSum = realSum };
        }

        Order o = _mapper.Map<Order>(order);
        OrderDTO createdOrder = _mapper.Map<OrderDTO>(await _orderRepository.AddOrder(o));

        await _kafkaProducer.PublishOrderCreatedAsync(createdOrder);

        return createdOrder;
    }

    public async Task<bool> UpdateOrderStatus(int id, string status)
    {
        var order = await _orderRepository.GetOrderById(id);
        if (order == null) return false;
        order.Status = status;
        return await _orderRepository.UpdateOrder(order);
    }
}
