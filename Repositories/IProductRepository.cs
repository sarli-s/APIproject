using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductById(int id);
        Task<(List<Product> Items, int TotalCount)> GetProducts(string? description, int? minPrice, int? maxPrice,
            int[]? categoryIds, int? limit, string? orderby, int? position);
        Task<Product> AddProduct(Product product);
        Task UpdateProduct(int id, Product product);
        Task DeleteProduct(int id);
    }
}
