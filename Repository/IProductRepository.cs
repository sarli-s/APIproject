using Entitys;

namespace Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(string? name,int? minPrice, int? maxprice, int[]? categoriesId,
            int? limit, string? orderby, int? offset);
    }
}