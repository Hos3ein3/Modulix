
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.BuildingBlocks.Context;
using ModuliX.Infrastructure.Context;

namespace ModuliX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, DeploymentConfigurations deploymentConfigurations)
    {

        if (deploymentConfigurations.DatabaseStrategy == BuildingBlocks.Enums.DatabaseStrategy.SingleDb)
        {
            services.AddScoped<IModuleDbContextConfigurator, ApplicationDbContextConfigurator>();
            services.AddAppDbContext<ApplicationDbContext>(configuration);
        }

        return services;
    }
}
