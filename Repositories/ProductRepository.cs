using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly dbSHOPContext _db;

        public ProductRepository(dbSHOPContext db) => _db = db;

        public async Task<Product> GetProductById(int id)
        {
            return await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<(List<Product> Items, int TotalCount)> GetProducts(string? description, int? minPrice,
            int? maxPrice, int[]? categoryIds, int? limit, string? orderby, int? position)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(description))
                query = query.Where(p => p.Description.Contains(description));
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);
            if (categoryIds != null && categoryIds.Length > 0)
                query = query.Where(p => categoryIds.Contains(p.CategoryId));

            query = query.OrderBy(p => p.Price);

            int total = await query.CountAsync();
            int pageSize = limit ?? 20;
            int page = position ?? 1;

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync();

            return (products, total);
        }

        public async Task<Product> AddProduct(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return await GetProductById(product.ProductId);
        }

        public async Task UpdateProduct(int id, Product product)
        {
            product.ProductId = id;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }
    }
}
