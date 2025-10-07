
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure PostgreSQL DbContexts.
/// Ensures consistent EF Core conventions across all modules.
/// </summary>
public static class PostgreSqlDefaultConfiguration
{
    /// <summary>
    /// Registers and configures a DbContext for PostgreSQL with common defaults.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">App configuration (used to read the connection string).</param>
    /// <param name="connectionName">The name of the connection string in appsettings.json.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPostgreSqlContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "PostgreSqlConnection")
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                // Automatically use the migrations assembly of the context
                npgsql.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                npgsql.CommandTimeout(60); // 60 seconds timeout by default
            });

            // Common EF Core conventions
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(true);
        });

        return services;
    }
}
