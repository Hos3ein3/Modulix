using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Enums;

namespace ModuliX.BuildingBlocks.Configurations;

public class DeploymentConfigurations
{
    public DeploymentMode DeploymentMode { get; set; }
    public DatabaseStrategy DatabaseStrategy { get; set; }

    public static IServiceCollection AddDeploymentConfigurations(IServiceCollection services, IConfiguration config)
    {
        services.Configure<DeploymentConfigurations>(
            config.GetSection(nameof(DeploymentConfigurations))
            );

        return services;
    }

    public static DeploymentConfigurations GetDeploymentConfigurations(IConfiguration config)
    {
        var deployment = new DeploymentConfigurations();
        config.GetSection(nameof(DeploymentConfigurations)).Bind(deployment);
        return deployment;
    }

}
