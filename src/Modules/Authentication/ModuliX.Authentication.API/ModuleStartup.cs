
using ModuliX.Authentication.API.Features;
using ModuliX.Authentication.Application;
using ModuliX.Authentication.Infrastructure;
using ModuliX.BuildingBlocks.Configurations;

namespace ModuliX.Authentication.API;

public static class ModuleStartup
{
    public static ConfigurationManager AddAuthenticationAppSettings(this ConfigurationManager config)
    {
        config.AddJsonFile("appsettings.Authentication.json", optional: false, reloadOnChange: true);
        return config;
    }
    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services, IConfiguration config)
    {

        services.AddAuthenticationApplication();
        // Register DbContext, services, etc.
        services.AddAuthenticationInfrastructure(config, DeploymentConfigurations.GetDeploymentConfigurations(config));
        return services;
    }


    public static IEndpointRouteBuilder UseAuthenticationModule(this IEndpointRouteBuilder app)
    {
        app.MapAuthenticationRouter();
        return app;
    }
}
