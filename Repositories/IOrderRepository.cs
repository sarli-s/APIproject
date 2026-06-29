using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<bool> UpdateOrder(Order order);
    }
}
