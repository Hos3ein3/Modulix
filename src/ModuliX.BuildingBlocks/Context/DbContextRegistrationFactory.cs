using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.BuildingBlocks.Enums;

namespace ModuliX.BuildingBlocks.Context;

public static class DbContextRegistrationFactory
{
    public static IServiceCollection RegisterModuleDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        DeploymentConfigurations deployment,
        string moduleName,
        string configSection = "Database")
        where TDbContext : DbContext
    {
        if (deployment.DatabaseStrategy == DatabaseStrategy.SingleDb)
        {
            // Always use default db in this case
            //return services.AddAppDbContext<TDbContext>(configuration, "Database");
            //this code register nothing but ModuliX.API in Register DBs
            return services;
        }

        // MultiDb → load module’s own config section from its own appsettings.{ModuleName}.json
        return services.AddAppDbContext<TDbContext>(configuration, $"{configSection}");
    }

    public static IServiceCollection RegisterAllDbContexts(
        this IServiceCollection services,
        IConfiguration configuration,
        DeploymentConfigurations deployment)
    {
        if (deployment.DeploymentMode == DeploymentMode.Monolith)
        {
            //return services.AddAppDbContext<TDbContext>(configuration, $"{configSection}");
            // services.RegisterModuleDbContext<AuthenticationDbContext>(
            //     configuration.GetSection("Database"), deployment, "Authentication");

            // services.RegisterModuleDbContext<UserManagementDbContext>(
            //     configuration.GetSection("Database"), deployment, "UserManagement");

            // add more here for other modules...
        }

        return services;
    }
}
