
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ClientFactories;

/// <summary>
/// Provides centralized registration and configuration for HttpClient instances.
/// This allows modules to make HTTP calls to external services or other internal APIs
/// with consistent timeout, headers, and retry policies.
/// </summary>
public static class HttpClientFactoryService
{
    /// <summary>
    /// Registers a named HttpClient with default configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">App configuration (used to read base URLs from appsettings).</param>
    /// <param name="clientName">Unique client name (used when retrieving with IHttpClientFactory).</param>
    /// <param name="configKey">
    /// The configuration key under "HttpClients" in appsettings.json that defines the base URL.
    /// Example: "HttpClients:NotificationService"
    /// </param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddHttpClientService(
        this IServiceCollection services,
        IConfiguration configuration,
        string clientName,
        string configKey)
    {
        var baseUrl = configuration[$"HttpClients:{configKey}"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException($"Missing base URL configuration for {configKey}");

        services.AddHttpClient(clientName, client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });

        return services;
    }

    /// <summary>
    /// Registers a typed HttpClient for a specific service interface or class.
    /// </summary>
    /// <typeparam name="TClient">The class that will use HttpClient via constructor injection.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="configKey">The key in appsettings.json that contains the base URL.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddTypedHttpClient<TClient>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configKey)
        where TClient : class
    {
        var baseUrl = configuration[$"HttpClients:{configKey}"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException($"Missing base URL configuration for {configKey}");

        services.AddHttpClient<TClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });

        return services;
    }
}
