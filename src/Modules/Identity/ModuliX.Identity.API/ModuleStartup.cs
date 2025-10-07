
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.Identity.API.Features;
using ModuliX.Identity.Application;
using ModuliX.Identity.Infrastructure;
using ModuliX.Identity.Persistence;

namespace ModuliX.Identity.API;

public static class ModuleStartup
{
    public static ConfigurationManager AddIdentityAppSettings(this ConfigurationManager config)
    {
        config.AddJsonFile("appsettings.Identity.json", optional: false, reloadOnChange: true);
        return config;
    }

    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration config)
    {

        services.AddIdentityApplication();
        services.AddIdentityInfrastructure(config, DeploymentConfigurations.GetDeploymentConfigurations(config));

        return services;
    }

    public static IEndpointRouteBuilder UseIdentityModule(this IEndpointRouteBuilder app)
    {
        app.MapIdentityRouter();
        return app;
    }
}

