using System.Text.Json;
using Entitys;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        dbSHOPContext _dbSHOPContext;

        public ProductRepository(dbSHOPContext dbSHOPContext)
        {
            _dbSHOPContext = dbSHOPContext;
        }

        public async Task<List<Product>> GetProducts(string? name, int? minPrice, int? maxprice, int[]? categoriesId,
            int? limit, string? orderby, int? offset)
        {
            //return await _dbSHOPContext.Products.Include(o => o.OrderItems).ThenInclude(o => o.Product).FirstOrDefaultAsync(o => o.OrderId == id);

            List<Product> s = await _dbSHOPContext.Products.ToListAsync();
            return s;
        }
    }
}
