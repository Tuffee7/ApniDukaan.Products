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
            var connectionStringTemplate = configuration.GetConnectionString("ProductsSqlConnection")!;

            var host = Environment.GetEnvironmentVariable("MSSQL_HOST") ?? string.Empty;
            var port = Environment.GetEnvironmentVariable("MSSQL_PORT") ?? string.Empty;
            var database = Environment.GetEnvironmentVariable("MSSQL_DATABASE") ?? string.Empty;
            var user = Environment.GetEnvironmentVariable("MSSQL_USER") ?? string.Empty;
            var password = Environment.GetEnvironmentVariable("MSSQL_PASSWORD") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(port) ||
                string.IsNullOrWhiteSpace(database) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("One or more MSSQL_* environment variables are missing. Ensure the selected launch profile provides MSSQL_HOST, MSSQL_PORT, MSSQL_DATABASE, MSSQL_USER and MSSQL_PASSWORD.");
            }

            var connectionString = connectionStringTemplate
                .Replace("$MSSQL_HOST", host)
                .Replace("$MSSQL_PORT", port)
                .Replace("$MSSQL_DATABASE", database)
                .Replace("$MSSQL_USER", user)
                .Replace("$MSSQL_PASSWORD", password);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
