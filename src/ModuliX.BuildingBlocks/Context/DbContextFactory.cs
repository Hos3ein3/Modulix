using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.BuildingBlocks.Enums;

namespace ModuliX.BuildingBlocks.Context;

public static class DbContextFactory
{
    public static IServiceCollection AddAppDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSection = "Database")
        where TDbContext : DbContext
    {
        var settings = new DatabaseConfigurations();

        // ✅ requires Microsoft.Extensions.Configuration.Binder
        configuration.GetSection(configSection).Bind(settings);

        services.Configure<DatabaseConfigurations>(configuration.GetSection(configSection));

        services.AddDbContext<TDbContext>((sp, options) =>
        {
            switch (settings.Provider)
            {
                case DatabaseProvider.SqlServer:
                    options.UseSqlServer(settings.ConnectionString);
                    break;

                case DatabaseProvider.PostgreSQL:
                    options.UseNpgsql(settings.ConnectionString);
                    break;

                case DatabaseProvider.MySql:
                    options.UseMySql(settings.ConnectionString, ServerVersion.AutoDetect(settings.ConnectionString));
                    break;

                case DatabaseProvider.Sqlite:
                    options.UseSqlite(settings.ConnectionString);
                    break;

                case DatabaseProvider.Oracle:
                    options.UseOracle(settings.ConnectionString);
                    break;

                case DatabaseProvider.InMemory:
                    //options.UseInMemoryDatabase("InMemoryDb");
                    break;

                case DatabaseProvider.Cosmos:
                    if (settings.ConnectionString == null)
                        throw new InvalidOperationException("Cosmos DB requires ConnectionString");
                    //options.UseCosmos(settings.ConnectionString, databaseName: "AppDatabase");
                    break;

                default:
                    throw new NotSupportedException($"Database provider {settings.Provider} is not supported.");
            }
        });

        return services;
    }
}
