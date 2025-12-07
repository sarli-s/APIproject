namespace Servers;

using Entitys;
using Repository;


public class CategoryService : ICategoryService
{
    private readonly ICategoriesRepository _categoryRepository;

    public CategoryService(ICategoriesRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _categoryRepository.GetCategories();
    }

}
