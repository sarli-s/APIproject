using Entitys;
using Repository;

namespace Servers
{
    public interface IOrdersService
    {

        Task<Order> AddOrder(Order order);
        Task<Order> GetOrderById(int id);
    }
}