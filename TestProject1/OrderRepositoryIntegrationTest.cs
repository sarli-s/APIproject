using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Repository;
using TestProject;

namespace TestProject1
{
    public class OrderRepositoryIntegrationTest : IClassFixture<DatabaseFixture>
    {
        private readonly dbSHOPContext _dbContext;
        private readonly OrderRepository _orderRepository;

        private Order GetOrderTest()
        {
            return new Order
            {
                UserId = 1,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 76,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1 },
                }
            };
        }
        private void ClearDatabase()
        {
            _dbContext.OrderItems.RemoveRange(_dbContext.OrderItems);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
        }

        public OrderRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _orderRepository = new OrderRepository(_dbContext);
        }

        [Fact]
        public async Task AddOrder_ShouldAddOrderToDatabase()
        {
            // Arrange
            ClearDatabase();

            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "password!@#" };
            var category = new Category { CategoryName = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var product = new Product { ProductName = "TestProduct", Price = 50, Category = category };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var order = GetOrderTest();

            // Act
            var result = await _orderRepository.AddOrder(order);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.OrderId > 0);

                var orderFromDb = await _dbContext.Orders.FindAsync(result.OrderId);
                Assert.NotNull(orderFromDb);
        }
        [Fact]
        public async Task GetOrderById_WhenOrderExists_ReturnsOrderWithItems()
        {
            // Arrange
            ClearDatabase();

            var user = new User { UserFirstName = "TestUser", UserEmail = "TestUser@.com", UserPassword = "password!@#" };
            var category = new Category { CategoryName = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var product = new Product { ProductName = "TestProduct", Price = 50, Category = category };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var order = GetOrderTest();


            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetOrderById(order.OrderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
            Assert.NotNull(result.OrderItems);
            Assert.Equal(1, result.OrderItems.Count);
        }

        [Fact]
        public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNull()
        {
            // Act
            ClearDatabase();

            var result = await _orderRepository.GetOrderById(9999);

            // Assert
            Assert.Null(result);
        }


    }
}
