namespace Servers;

using Entitys;
using Repository;


public class PrudectsService : IPrudectsService
{
    private readonly IProductRepository _productRepository;

    public PrudectsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetProducts(string? name, int? minPrice, int? maxprice, int[]? categoriesId,
            int? limit, string? orderby, int? offset)
    {
        return await _productRepository.GetProducts(name,minPrice,maxprice, categoriesId,limit,orderby,offset);
    }

}
