
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure Oracle Database DbContexts.
/// Ensures consistent EF Core conventions across all modules.
/// </summary>
public static class OracleDbDefaultConfiguration
{
    /// <summary>
    /// Registers and configures a DbContext for Oracle Database with standard defaults.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration (used to read the connection string).</param>
    /// <param name="connectionName">The name of the connection string in appsettings.json.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddOracleDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "OracleConnection")
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        services.AddDbContext<TContext>(options =>
        {
            options.UseOracle(connectionString, oracle =>
            {
                // Automatically use the migrations assembly of the context
                oracle.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                oracle.CommandTimeout(60); // default timeout in seconds
            });

            // Common EF Core conventions
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(true);
        });

        return services;
    }
}
