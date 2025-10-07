using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.Identity.Persistence;

namespace ModuliX.Authentication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            DeploymentConfigurations deployment)
    {
        // Register persistence from Identity
        services.AddIdentityPersistence(configuration, deployment);

        // Add other Authentication infra services (e.g. token generator)
        //services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
