using DTOs;
using Entitys;
using Repository;

namespace Servers
{
    public interface IOrdersService
    {

        Task<OrderDTO> AddOrder(OrderDTO order);
        Task<OrderDTO> GetOrderById(int id);
    }
}