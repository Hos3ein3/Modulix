
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.BuildingBlocks.Context;
using ModuliX.Identity.Persistence.Context;

namespace ModuliX.Identity.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        DeploymentConfigurations deployment)
        {
            services.AddScoped<IModuleDbContextConfigurator, IdentityDbConfigurator>();
            services.RegisterModuleDbContext<ApplicationIdentityDbContext>(
                configuration,
                deployment,
                moduleName: "Identity");


            return services;
        }
    }
}
