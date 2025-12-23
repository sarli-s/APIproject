namespace Servers;

using AutoMapper;
using DTOs;
using Entitys;
using Repository;


public class PrudectsService : IPrudectsService
{
    private readonly IProductRepository _productRepository;
    IMapper _mapper;

    public PrudectsService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDTO>> GetProducts(string? name, int? minPrice, int? maxprice, int[]? categoriesId,
            int? limit, string? orderby, int? offset)
    {
        return _mapper.Map<List<Product>, List<ProductDTO>>(await _productRepository.GetProducts(name, minPrice, maxprice, categoriesId, limit, orderby, offset));
    }

}
