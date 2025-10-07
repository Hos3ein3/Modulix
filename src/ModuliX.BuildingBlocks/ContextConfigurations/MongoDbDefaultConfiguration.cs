
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ModuliX.BuildingBlocks.ContextConfigurations;

/// <summary>
/// Provides a centralized way to configure MongoDB connections and databases.
/// Used for document-oriented modules or microservices.
/// </summary>
public static class MongoDbDefaultConfiguration
{
    /// <summary>
    /// Registers a MongoDB client and optionally a specific database for dependency injection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration (used to read the connection string).</param>
    /// <param name="connectionName">The name of the connection string in appsettings.json.</param>
    /// <param name="databaseName">Optional: name of the MongoDB database to bind directly.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMongoDbConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionName = "MongoDbConnection",
        string? databaseName = null)
    {
        var connectionString = configuration.GetConnectionString(connectionName);
        var mongoUrl = new MongoUrl(connectionString);

        // Register a singleton MongoClient (thread-safe and reused across app)
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoUrl));

        // Optionally register a default IMongoDatabase for quick injection
        if (!string.IsNullOrWhiteSpace(databaseName))
        {
            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });
        }

        return services;
    }
}
