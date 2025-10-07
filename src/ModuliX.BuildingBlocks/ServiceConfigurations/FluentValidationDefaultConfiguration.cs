
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace ModuliX.BuildingBlocks.ServiceConfigurations;

/// <summary>
/// Provides centralized configuration for FluentValidation across all modules.
/// </summary>
public static class FluentValidationServiceConfiguration
{
    /// <summary>
    /// Registers FluentValidation for API and Application layers with MediatR.
    /// <code> services.AddTransient(typeof(ValidationBehavior<,>), typeof(ValidationBehavior<,>)); </code>
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies containing validators.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDefaultFluentValidationNeedMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Automatically register validators from given assemblies
        if (assemblies != null && assemblies.Any())
        {
            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        }
        else
        {
            // Default behavior: scan all loaded assemblies
            services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());
        }

        return services;
    }

    /// <summary>
    /// Registers FluentValidation for API and MVC controllers.
    /// <code> services.AddTransient(typeof(ValidationBehavior<,>), typeof(ValidationBehavior<,>)); </code>
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies containing validators.</param>
    public static IServiceCollection AddDefaultFluentValidation(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        // Add FluentValidation integration for ASP.NET Core
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Automatically scan and register validators
        if (assemblies != null && assemblies.Any())
            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        else
            services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());

        return services;
    }
}
