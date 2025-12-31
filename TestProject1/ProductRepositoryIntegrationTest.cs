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
    public class ProductRepositoryIntegrationTest : IClassFixture<DatabaseFixture>
    {
        private readonly dbSHOPContext _dbContext;
        private readonly ProductRepository _productRepository;
        public ProductRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _productRepository = new ProductRepository(_dbContext);
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
        [Fact]
        public async Task GetProducts_WhenDataExists_ReturnsAllProductsWithCategory()
        {
            // Arrange
            ClearDatabase();
            var category = new Category { CategoryName = "TestCategory" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            var testProducts = new List<Product>
            {
                new Product { ProductName = "Product1", Price = 10, CategoryId = category.CategoryId, Description = "Desc1" },
                new Product { ProductName = "Product2", Price = 20, CategoryId = category.CategoryId, Description = "Desc2" }
            };
            await _dbContext.Products.AddRangeAsync(testProducts);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _productRepository.GetProducts(null, null, null, null, null, null, null);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(testProducts.Count, result.Count);
            Assert.All(result, p => Assert.NotNull(p.Category));
            foreach (var product in testProducts)
            {
                Assert.Contains(result, p => p.ProductName == product.ProductName && p.Category != null);
            }
        }

        [Fact]
        public async Task GetProducts_ReturnsEmpty_WhenNoDataExists()
        {
            // Arrange
            ClearDatabase();
            // Act
            var result = await _productRepository.GetProducts(null, null, null, null, null, null, null);
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
