using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Repository;
using TestProject;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace TestProject1
{
    public class CategoryRepositoryIntegrationTest : IClassFixture<DatabaseFixture>, IDisposable
    {

        private readonly dbSHOPContext _dbContext;
        private readonly CategoriesRepository _categoryRepository;
        public CategoryRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _categoryRepository = new CategoriesRepository(_dbContext);
            ClearDatabase();
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
        public async Task GetCategories_ReturnsAllCategories_whenDataExsists()
        {

            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryName = "Electronics" },
                new Category { CategoryName = "Books" },
                new Category { CategoryName = "Clothing" }
            };

            _dbContext.Categories.AddRange(categories);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.CategoryName == "Electronics");
            Assert.Contains(result, c => c.CategoryName == "Books");
            Assert.Contains(result, c => c.CategoryName == "Clothing");
        }
        [Fact]
        public async Task GetCategories_ReturnsEmpty_WhenNoDataExists()
        {
            // Arrange


            // Act
            var result = await _categoryRepository.GetCategories();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            ClearDatabase();
        }
    }
}
