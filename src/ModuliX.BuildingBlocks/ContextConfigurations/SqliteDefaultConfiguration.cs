

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure SQLite DbContexts.
/// Used for lightweight or local module databases.
/// <code> builder.Services.AddSqliteContext<MyModuleDbContext>(builder.Configuration); </code>
/// Or Named Connection
/// <code>builder.Services.AddSqliteContext<MyModuleDbContext>(builder.Configuration, "LocalSqliteConnection");</code> 
/// </summary>
public static class SqliteDefaultConfiguration
{
    /// <summary>
    /// Registers and configures a DbContext for SQLite with default conventions.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">App configuration (used to read the connection string).</param>
    /// <param name="connectionName">The name of the connection string in appsettings.json.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSqliteContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "DefaultConnection")
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlite(connectionString, sqlite =>
            {
                sqlite.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                sqlite.CommandTimeout(60); // default command timeout
            });

            // Common EF Core conventions
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(true);
        });

        return services;
    }
}
