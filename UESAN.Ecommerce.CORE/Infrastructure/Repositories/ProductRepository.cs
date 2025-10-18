using Microsoft.EntityFrameworkCore;
using UESAN.Ecommerce.CORE.Core.Entities;
using UESAN.Ecommerce.CORE.Core.Interfaces;
using UESAN.Ecommerce.CORE.Infrastructure.Data;

namespace UESAN.Ecommerce.CORE.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreDbContext _context;

        public ProductRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            // Include Category navigation so service can map nested Category
            return await _context.Product.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Product.Include(p => p.Category).Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> InsertProduct(Product product)
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateProduct(Product product)
        {
            var existing = await _context.Product.FindAsync(product.Id);
            if (existing != null)
            {
                existing.Description = product.Description;
                existing.ImageUrl = product.ImageUrl;
                existing.Stock = product.Stock;
                existing.Price = product.Price;
                existing.Discount = product.Discount;
                existing.CategoryId = product.CategoryId;
                existing.IsActive = product.IsActive;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
