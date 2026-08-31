using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.Business.Mappers;
using Products.Business.ServiceContracts;
using Products.Business.Services;
using Products.Business.Validators;

namespace Products.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            // TODO: Add your business logic layer services here into the IoC container:

            services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly); // Register AutoMapper profiles from this assembly

            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

            services.AddScoped<IProductService, ProductService>(); // Register ProductService with its interface

            return services;
        }
    }
}
