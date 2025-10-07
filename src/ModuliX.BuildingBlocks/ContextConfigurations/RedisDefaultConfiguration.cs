

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure Redis connections and caching.
/// Used for distributed caching, pub/sub, or message queueing across modules.
/// <code> builder.Services.AddRedisConfiguration(builder.Configuration); </code>
/// </summary>
public static class RedisDefaultConfiguration
{
    /// <summary>
    /// Registers a shared Redis connection multiplexer and optional distributed cache.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">App configuration (to read the connection string).</param>
    /// <param name="connectionName">The name of the Redis connection string in appsettings.json.</param>
    /// <param name="enableDistributedCache">Whether to register IDistributedCache using Redis.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRedisConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "Redis",
        bool enableDistributedCache = true)
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        // Shared connection multiplexer (thread-safe and reused across the app)
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(connectionString));

        // Optionally register as a distributed cache provider
        if (enableDistributedCache)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = "ModuliX_";
            });
        }

        return services;
    }
}
