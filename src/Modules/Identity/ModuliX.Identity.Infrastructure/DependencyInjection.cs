
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.Identity.Domain.Models;
using ModuliX.Identity.Persistence;
using ModuliX.Identity.Persistence.Context;

namespace ModuliX.Identity.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration, DeploymentConfigurations deploymentConfigurations)
    {
        services.AddIdentityPersistence(configuration, deploymentConfigurations);

        services.AddIdentityCore<AspNetUser>(options =>
{
    // ✅ Password policy
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // ✅ User policy
    options.User.RequireUniqueEmail = true;

    // ✅ Lockout policy
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    // ✅ Sign-in policy
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddRoles<AspNetRole>()                      // if you use roles
.AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddUserManager<UserManager<AspNetUser>>()
.AddRoleManager<RoleManager<AspNetRole>>();



        return services;
    }
}

