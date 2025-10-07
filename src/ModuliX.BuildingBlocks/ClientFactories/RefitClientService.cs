
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Refit;
// using Refit.HttpClientFactory;

// namespace ModuliX.BuildingBlocks.ClientFactories;

// /// <summary>
// /// Provides a centralized way to register and configure Refit HTTP clients.
// /// This allows modules to communicate via REST APIs using strongly-typed interfaces.
// /// </summary>
// public static class RefitClientService
// {
//     /// <summary>
//     /// Registers a Refit client for the specified API interface type.
//     /// </summary>
//     /// <typeparam name="TClient">The Refit interface representing the API client.</typeparam>
//     /// <param name="services">The service collection.</param>
//     /// <param name="configuration">App configuration (used to retrieve base URL).</param>
//     /// <param name="configKey">
//     /// The configuration key in appsettings.json under "RefitClients" that defines the base URL.
//     /// Example: "RefitClients:UserService" → base URL for User Service API.
//     /// </param>
//     /// <returns>The updated service collection.</returns>
//     public static IServiceCollection AddRefitClientService<TClient>(
//         this IServiceCollection services,
//         IConfiguration configuration,
//         string configKey)
//         where TClient : class
//     {
//         var baseUrl = configuration[$"RefitClients:{configKey}"];

//         if (string.IsNullOrWhiteSpace(baseUrl))
//             throw new ArgumentException($"Missing base URL configuration for {configKey}");

//         services.AddRefitClient<TClient>()
//             .ConfigureHttpClient(client =>
//             {
//                 client.BaseAddress = new Uri(baseUrl);
//                 client.Timeout = TimeSpan.FromSeconds(30);
//                 client.DefaultRequestHeaders.Add("Accept", "application/json");
//             });

//         return services;
//     }
// }
