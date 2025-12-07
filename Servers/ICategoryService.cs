using Entitys;
using Repository;

namespace Servers
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
    }
}