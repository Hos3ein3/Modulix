
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using ModuliX.BuildingBlocks.Configurations;

namespace ModuliX.BuildingBlocks.ServiceConfigurations;

public static class FeatureManagementDefaultConfiguration
{

    public static IServiceCollection AddDefaultFeatureManagement(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddFeatureManagement();

        var featureConfig = FeatureManagementConfiguration.FromConfiguration(configuration);
        services.AddSingleton(featureConfig);

        return services;
    }

}

