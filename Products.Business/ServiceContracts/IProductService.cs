using Products.Business.RequestDto;
using Products.Business.ResponseDto;
using Products.DataLayer.Entities;
using System.Linq.Expressions;

namespace Products.Business.ServiceContracts
{
    public interface IProductService
    {
        /// <summary>
        /// Gets the list of products.
        /// </summary>
        /// <returns></returns>
        Task<List<ProductResponse?>> GetProducts();

        /// <summary>
        /// Gets the list of products by condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<ProductResponse?>?> GetProductsByCondition(Expression<Func<Product, bool>> condition);

        /// <summary>
        /// Gets a single product by condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<ProductResponse?> GetSingleProductByCondition(Expression<Func<Product, bool>> condition);

        /// <summary>
        /// Adds a new Products.
        /// </summary>
        /// <param name="productAddRequest"></param>
        /// <returns></returns>
        Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);
        /// <summary>
        /// Updates an existing Products.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productUpdateRequest"></param>
        /// <returns></returns>
        Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);
        /// <summary>
        /// Deletes a Products.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(Guid id);
    }
}
