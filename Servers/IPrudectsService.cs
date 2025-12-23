using DTOs;
using Entitys;
using Repository;

namespace Servers
{
    public interface IPrudectsService
    {
        Task<List<ProductDTO>> GetProducts(string? name, int? minPrice, int? maxprice, int[]? categoriesId,
            int? limit, string? orderby, int? offset);

    }
}