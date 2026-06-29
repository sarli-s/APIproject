using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly dbSHOPContext _db;

        public OrderRepository(dbSHOPContext db) => _db = db;

        public async Task<Order> GetOrderById(int id)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<Order> AddOrder(Order order)
        {
            await _db.AddAsync(order);
            await _db.SaveChangesAsync();
            return await GetOrderById(order.OrderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
