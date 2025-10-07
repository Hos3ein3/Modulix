
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ServiceConfigurations;

/// <summary>
/// Registers MediatR and CQRS services globally.
/// </summary>
public static class CQRSDefaultConfiguration
{
    /// <summary>
    /// Adds MediatR and automatically scans for handlers and notifications.
    /// <code> services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); </code>
    /// </summary>
    public static IServiceCollection AddDefaultCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

        services.AddMediatR(assemblies);
        return services;
    }
}
