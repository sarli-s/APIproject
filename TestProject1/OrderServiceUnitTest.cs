using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Repositories;
using Servers;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class OrderServiceUnitTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<OrdersService>> _loggerMock;
        private readonly Mock<IKafkaProducerService> _kafkaProducerMock;
        private readonly OrdersService _ordersService;

        public OrderServiceUnitTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OrdersService>>();
            _kafkaProducerMock = new Mock<IKafkaProducerService>();

            _ordersService = new OrdersService(
                _orderRepositoryMock.Object,
                _mapperMock.Object,
                _productRepositoryMock.Object,
                _loggerMock.Object,
                _kafkaProducerMock.Object
            );
        }

        // 2 units of product 1
        private static List<OrderItemDTO> TwoUnitsOfProduct1() =>
            new List<OrderItemDTO> { new OrderItemDTO(0, 1, "Product", 2, "red", "", 0) };

        private static OrderDTO NewOrder(int orderId, double sum, List<OrderItemDTO> items) =>
            new OrderDTO(orderId, DateOnly.FromDateTime(DateTime.Now), items, 1, OrderSum: sum);

        [Fact]
        public async Task AddOrder_SumIsCorrect_ReturnsOrderDto()
        {
            // Arrange - Happy Path
            var items = TwoUnitsOfProduct1();
            OrderDTO orderDto = NewOrder(0, 100, items);

            Product product = new Product { ProductId = 1, Price = 50 };
            Order orderEntity = new Order { OrderSum = 100 };
            OrderDTO savedOrderDto = NewOrder(1, 100, items);


            _productRepositoryMock.Setup(r => r.GetProductById(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<Order>(It.IsAny<object>())).Returns(orderEntity);
            _orderRepositoryMock.Setup(r => r.AddOrder(orderEntity)).ReturnsAsync(orderEntity);
            _mapperMock.Setup(m => m.Map<OrderDTO>(orderEntity)).Returns(savedOrderDto);

            // Act
            var result = await _ordersService.AddOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.OrderSum);
            _kafkaProducerMock.Verify(k => k.PublishOrderCreatedAsync(savedOrderDto), Times.Once);

            // בדיקה שהלוגר מעולם לא הופעל
            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task AddOrder_SumMismatch_CorrectsSumAndLogsWarning()
        {
            // Arrange - Unhappy Path
            var items = TwoUnitsOfProduct1();
            OrderDTO orderDto = NewOrder(0, 50, items);

            var product = new Product { ProductId = 1, Price = 60 };
            var orderEntity = new Order();
            OrderDTO savedOrderDto = NewOrder(1, 120, items);

            _productRepositoryMock.Setup(r => r.GetProductById(1)).ReturnsAsync(product);

            // כאן אנחנו מוודאים שה-Mapper מקבל אובייקט שהסכום שלו כבר תוקן ל-120
            _mapperMock.Setup(m => m.Map<Order>(It.Is<object>(o => ((OrderDTO)o).OrderSum == 120)))
                       .Returns(orderEntity);

            _orderRepositoryMock.Setup(r => r.AddOrder(orderEntity)).ReturnsAsync(orderEntity);
            _mapperMock.Setup(m => m.Map<OrderDTO>(orderEntity)).Returns(savedOrderDto);

            // Act
            var result = await _ordersService.AddOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(120, result.OrderSum);

            // בדיקה שהלוגר אכן הופעל
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Order sum mismatch")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
