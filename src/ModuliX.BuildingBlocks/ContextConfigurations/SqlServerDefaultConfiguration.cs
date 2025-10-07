

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure SQL Server DbContexts.
/// Use this to ensure all modules follow the same EF Core conventions and settings.
/// <code> builder.Services.AddSqlServerContext<ContextDbContext>(builder.Configuration); </code>
/// Or Named Connection
/// <code> services.AddSqlServerContext<AuthDbContext>(configuration, "AuthConnection"); </code>
/// </summary>
public static class SqlServerDefaultConfiguration
{
    /// <summary>
    /// Registers and configures a DbContext for SQL Server with default conventions.
    /// </summary>
    /// <typeparam name="TContext">The DbContext type to register.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">App configuration (used to read the connection string).</param>
    /// <param name="connectionName">The name of the connection string in appsettings.json.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSqlServerContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "DefaultConnection")
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                sql.CommandTimeout(60); // default command timeout
            });

            // Common EF Core conventions
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(true);
        });

        return services;
    }
}
