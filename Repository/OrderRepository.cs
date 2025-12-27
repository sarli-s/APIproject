using System.Text.Json;
using Entitys;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        dbSHOPContext _dbSHOPContext;
        public OrderRepository(dbSHOPContext dbSHOPContext)
        {
            _dbSHOPContext = dbSHOPContext;
        }

        public async Task<Order> GetOrderById(int id)
        {
            //return await _dbSHOPContext.FindAsync<Order>(id);
            Order order = await _dbSHOPContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == id);
            return order;
        }


        public async Task<Order> AddOrder(Order order)
        {
            await _dbSHOPContext.AddAsync(order);
            await _dbSHOPContext.SaveChangesAsync();
            return await _dbSHOPContext.Orders.FindAsync(order.OrderId);
        }

    }
}
