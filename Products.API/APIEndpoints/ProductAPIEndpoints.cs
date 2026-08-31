using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.Business.RequestDto;
using Products.Business.ResponseDto;
using Products.Business.ServiceContracts;
using Products.DataLayer.Entities;

namespace Products.API.APIEndpoints
{
    public static class ProductAPIEndpoints
    {
        public static IEndpointRouteBuilder MapProductAPIEndPoints(this IEndpointRouteBuilder app)
        {
            //GET /api/products
            app.MapGet("/api/products", async (IProductService productService) =>
            {
                List<ProductResponse?> products = await productService.GetProducts();

                return Results.Ok(products);
            });

            //GET /api/products/search/product-id/00000000-0000-0000-0000-000000000000
            app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
            {
                ProductResponse? product = await productService.GetSingleProductByCondition(x => x.ProductID == ProductID);

                if (product == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(product);
            });

            //GET /api/products/search/id/xxxxxxx
            app.MapGet("/api/products/search/{searchString}", async (IProductService productService, string searchString) =>
            {
                List<ProductResponse?> productsByProductName = await productService.GetProductsByCondition(x => !string.IsNullOrEmpty(x.ProductName)
                                                                                              && EF.Functions.Like(x.ProductName, $"%{searchString}%"));

                List<ProductResponse?> productsByCategory = await productService.GetProductsByCondition(x => !string.IsNullOrEmpty(x.Category)
                                                                                              && EF.Functions.Like(x.Category, $"%{searchString}%"));

                //List<ProductResponse?> productsByPrice = await productService.GetProductsByCondition(x => x.Price.HasValue 
                //                                                                              && EF.Functions.Like(x.Price.ToString(), $"%{searchString}%"));

                List<ProductResponse?> products = new List<ProductResponse?>();
                products.AddRange(productsByProductName);
                products.AddRange(productsByCategory);
                //products.AddRange(productsByPrice);

                if (products == null || products.Count == 0)
                {
                    return Results.NotFound();
                }

                return Results.Ok(products);
            });

            //POST /api/products
            app.MapPost("/api/products", async (IProductService productService, IValidator<ProductAddRequest> validator, ProductAddRequest productAddRequest) =>
            {
                var validationResult = await validator.ValidateAsync(productAddRequest);
                if (!validationResult.IsValid)
                {
                    var validationErrors = validationResult.Errors.GroupBy(x => x.PropertyName)
                                           .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
                    return Results.ValidationProblem(validationErrors);
                }

                var productResponse = await productService.AddProduct(productAddRequest);

                if (productResponse != null)
                {
                    return Results.Created($"/api/products/search/product-id/{productResponse.ProductID}", productResponse);
                }
                else
                {
                    return Results.Problem("Failed to add product.");
                }

            });

            //PUT /api/products
            app.MapPut("/api/products", async (IProductService productService, IValidator<ProductUpdateRequest> validator, ProductUpdateRequest productUpdateRequest) =>
            {
                var validationResult = await validator.ValidateAsync(productUpdateRequest);
                if (!validationResult.IsValid)
                {
                    var validationErrors = validationResult.Errors.GroupBy(x => x.PropertyName)
                                           .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
                    return Results.ValidationProblem(validationErrors);
                }

                var updateProductResponse = await productService.UpdateProduct(productUpdateRequest);

                if (updateProductResponse != null)
                {
                    return Results.Ok(updateProductResponse);
                }
                else
                {
                    return Results.Problem("Failed to update product.");
                }

            });

            //DELETE /api/products/00000000-0000-0000-0000-000000000000
            app.MapDelete("/api/products/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
            {
                var result = await productService.DeleteProduct(ProductID);
                if (result)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.Problem("Failed to delete product.");
                }
            });

            return app;
        }
    }
}
