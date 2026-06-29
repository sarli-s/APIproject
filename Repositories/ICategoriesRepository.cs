using Entities;

namespace Repositories
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryByName(string name);
    }
}
