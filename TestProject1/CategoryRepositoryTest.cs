using Entitys;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;

namespace TestProject1
{
    public class CategoryRepositoryTest
    {
        [Fact]
        
        public async Task GetCategory_ValidCredentials_ReturnsCategory()
        {
            // Arrange
            var category1 = new Category { CategoryName = "toys" };
            var category2 = new Category { CategoryName = "books" };

            var mockContext = new Mock<dbSHOPContext>();
            var categories = new List<Category>() { category1, category2 };
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);

            var categoryRepository = new CategoriesRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.Equal(categories, result);
        }
    }
}
