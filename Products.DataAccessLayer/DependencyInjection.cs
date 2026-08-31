using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Data.DatabaseContext;
using Products.Data.Repositories;
using Products.Data.RepositoryContracts;

namespace Products.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: Add your data access layer services here into the IoC container:
            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
