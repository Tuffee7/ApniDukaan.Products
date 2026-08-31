using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Products.Business.RequestDto;
using Products.Business.ResponseDto;
using Products.Business.ServiceContracts;
using Products.Data.RepositoryContracts;
using Products.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Products.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IValidator<ProductAddRequest> _productAddValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateValidator;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IValidator<ProductAddRequest> productAddValidator, 
                              IValidator<ProductUpdateRequest> productUpdateValidator, 
                              IMapper mapper, 
                              IProductRepository productRepository)
        {
            _productAddValidator = productAddValidator;
            _productUpdateValidator = productUpdateValidator;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Adds a new product to the repository after validating the input request.
        /// </summary>
        /// <param name="productAddRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            if (productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest), "Product add request cannot be null");
            }

            // Validate the productAddRequest using FluentValidation
            ValidationResult validationResult = await _productAddValidator.ValidateAsync(productAddRequest);
            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                throw new ArgumentException(errorMessage);
            }

            // Map the ProductAddRequest to Product entity
            var product = _mapper.Map<Product>(productAddRequest);
            if (product == null)
            {
                throw new InvalidOperationException("Mapping from ProductAddRequest to Product failed.");
            }

            // Add the product to the repository
            var addedProduct = await _productRepository.AddProductAsync(product);

            if (addedProduct == null)
            {
                throw new InvalidOperationException("Failed to add the product.");
            }

            // Map the added Product entity back to ProductResponse
            return _mapper.Map<ProductResponse>(addedProduct);
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _productRepository.GetSingleProductByConditionAsync(p => p.ProductID == id);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            return await _productRepository.DeleteProductAsync(id);
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            IEnumerable<Product?> products = await _productRepository.GetProductsAsync();
            
            if (products == null)
                return null;

            IEnumerable<ProductResponse?> productResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);

            return productResponse.ToList();
        }

        public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
        {
            IEnumerable<Product?> products = await _productRepository.GetProductsByConditionAsync(condition);
            if (products == null)
                return null;

            IEnumerable<ProductResponse?> productResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);

            return productResponse.ToList();
        }

        public async Task<ProductResponse?> GetSingleProductByCondition(Expression<Func<Product, bool>> condition)
        {
            Product? product = await _productRepository.GetSingleProductByConditionAsync(condition);

            if (product == null)
                return null;

            ProductResponse? productResponse = _mapper.Map<ProductResponse>(product);

            return productResponse;
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            Product? existingProduct = await _productRepository.GetSingleProductByConditionAsync(p => p.ProductID == productUpdateRequest.ProductID);
            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            var validationResult = await _productUpdateValidator.ValidateAsync(productUpdateRequest);
            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                throw new ArgumentException(errorMessage);
            }

            // Map the ProductUpdateRequest to the existing Product entity
            Product? product = _mapper.Map<Product>(productUpdateRequest);

            // Update the product in the repository
            var updatedProduct = await _productRepository.UpdateProductAsync(product);

            if (updatedProduct == null)
            {
                throw new InvalidOperationException("Failed to update the product.");
            }

            // Map the updated Product entity back to ProductResponse
            return _mapper.Map<ProductResponse?>(updatedProduct);
        }
    }
}
