using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly dbSHOPContext _db;

        public CategoriesRepository(dbSHOPContext db) => _db = db;

        public async Task<List<Category>> GetCategories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
        }
    }
}
