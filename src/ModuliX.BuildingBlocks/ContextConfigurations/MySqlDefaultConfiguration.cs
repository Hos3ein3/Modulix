

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure MySQL DbContexts.
/// Ensures consistent EF Core conventions across all modules.
/// </summary>
public static class MySqlDefaultConfiguration
{
    /// <summary>
    /// Registers and configures a DbContext for MySQL with sensible defaults.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">App configuration (to read the connection string).</param>
    /// <param name="connectionName">The name of the connection string in appsettings.json.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMySqlContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "MySqlConnection")
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        services.AddDbContext<TContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySql =>
            {
                // Automatically use the migrations assembly of the context
                mySql.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                mySql.CommandTimeout(60); // default timeout in seconds
            });

            // Common EF Core conventions
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(true);
        });

        return services;
    }
}
