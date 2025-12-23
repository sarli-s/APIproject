namespace Servers;

using AutoMapper;
using DTOs;
using Entitys;
using Repository;


public class OrdersService : IOrdersService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrdersService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDTO> GetOrderById(int id)
    {
        return _mapper.Map<Order, OrderDTO>( await _orderRepository.GetOrderById(id));
    }

    public async Task<OrderDTO> AddOrder(OrderDTO order)
    {
        return _mapper.Map < Order, OrderDTO > (await _orderRepository.AddOrder(_mapper.Map <OrderDTO, Order>(order)));

    }
}
