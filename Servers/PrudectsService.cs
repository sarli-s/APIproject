namespace Servers;

using AutoMapper;
using DTOs;
using Entities;
using Repositories;

public class PrudectsService : IPrudectsService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoriesRepository _categoryRepository;
    private readonly ISearchService _searchService;
    private readonly IMapper _mapper;

    public PrudectsService(
        IProductRepository productRepository,
        ICategoriesRepository categoryRepository,
        ISearchService searchService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _searchService = searchService;
        _mapper = mapper;
    }

    public async Task<PageResponseDTO<ProductDTO>> GetProducts(string? description, int? minPrice, int? maxPrice,
        int[]? categoriesId, int? limit, string? orderby, int? offset)
    {
        (List<Product> items, int totalCount) = await _productRepository.GetProducts(
            description, minPrice, maxPrice, categoriesId, limit, orderby, offset);

        return new PageResponseDTO<ProductDTO>
        {
            Data = _mapper.Map<List<ProductDTO>>(items),
            TotalItems = totalCount,
            CurrentPage = offset ?? 1,
            PageSize = limit ?? 20,
            HasPreviousPage = (offset ?? 1) > 1,
            HasNextPage = (offset ?? 1) * (limit ?? 20) < totalCount
        };
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await _productRepository.GetProductById(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> AddProduct(ProductDTO productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var category = await _categoryRepository.GetCategoryByName(productDto.Category.CategoryName);
        if (category == null) throw new Exception("Category not found");
        product.CategoryId = category.CategoryId;

        var created = await _productRepository.AddProduct(product);
        await _searchService.SeedAsync();
        return _mapper.Map<ProductDTO>(created);
    }

    public async Task<ProductDTO> UpdateProduct(int id, ProductDTO productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var category = await _categoryRepository.GetCategoryByName(productDto.Category.CategoryName);
        if (category != null) product.CategoryId = category.CategoryId;

        await _productRepository.UpdateProduct(id, product);
        await _searchService.SeedAsync();
        return productDto;
    }

    public async Task DeleteProduct(int id)
    {
        await _productRepository.DeleteProduct(id);
        await _searchService.SeedAsync();
    }
}
