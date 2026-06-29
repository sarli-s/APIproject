using DTOs;

namespace Servers
{
    public interface IPrudectsService
    {
        Task<PageResponseDTO<ProductDTO>> GetProducts(string? name, int? minPrice, int? maxPrice,
            int[]? categoriesId, int? limit, string? orderby, int? offset);
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> AddProduct(ProductDTO productDto);
        Task<ProductDTO> UpdateProduct(int id, ProductDTO productDto);
        Task DeleteProduct(int id);
    }
}
