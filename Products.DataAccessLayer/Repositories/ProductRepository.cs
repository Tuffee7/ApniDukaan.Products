using Microsoft.EntityFrameworkCore;
using Products.Data.DatabaseContext;
using Products.Data.RepositoryContracts;
using Products.DataLayer.Entities;
using System.Linq.Expressions;

namespace Products.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext) 
        { 
            _dbContext = applicationDbContext;
        }

        public async Task<Product?> AddProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product == null)
            {
                return false;
            }

            _dbContext.Products.Remove(product);
            int affectedRows = await _dbContext.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _dbContext.Products.Where(conditionExpression).ToListAsync();
        }

        public async Task<Product?> GetSingleProductByConditionAsync(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(conditionExpression);
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

            if (existingProduct == null)
            {
                return null;
            }
            
            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
            existingProduct.QuantityInStock = product.QuantityInStock;
            existingProduct.Category = product.Category;

            await _dbContext.SaveChangesAsync();

            return existingProduct;
        }
    }
}
