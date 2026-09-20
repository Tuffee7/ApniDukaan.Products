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
            string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;

            string connectionString = connectionStringTemplate;

            string? mssqlHost = Environment.GetEnvironmentVariable("MSSQL_HOST");
            string? mssqlPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");

            if (!string.IsNullOrEmpty(mssqlHost))
            {
                connectionString = connectionString.Replace("$MSSQL_HOST", mssqlHost);
            }

            if (!string.IsNullOrEmpty(mssqlPassword))
            {
                connectionString = connectionString.Replace("$MSSQL_SA_PASSWORD", mssqlPassword);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
