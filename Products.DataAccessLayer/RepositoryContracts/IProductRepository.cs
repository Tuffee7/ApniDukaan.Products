using Products.DataLayer.Entities;
using System.Linq.Expressions;

namespace Products.Data.RepositoryContracts
{
    public interface IProductRepository
    {
        /// <summary>
        /// Gets all products from the data source.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsAsync();

        /// <summary>
        /// Gets products from the data source based on a condition expression.
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<IEnumerable<Product?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> conditionExpression);

        /// <summary>
        /// Gets a single product from the data source based on a condition expression.
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<Product?> GetSingleProductByConditionAsync(Expression<Func<Product, bool>> conditionExpression);

        /// <summary>
        /// Adds a new product to the data source.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product?> AddProductAsync(Product product);

        /// <summary>
        /// Updates an existing product in the data source.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product?> UpdateProductAsync(Product product);

        /// <summary>
        /// Deletes a product from the data source.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
