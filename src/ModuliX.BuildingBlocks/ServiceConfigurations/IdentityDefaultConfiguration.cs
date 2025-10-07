
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.ServiceConfigurations;

//TODO: Add UserManager, RoleManager, SignInManager configurations
/// <summary>
/// Provides a centralized configuration for ASP.NET Core Identity.
/// This setup does not register any specific data store (e.g., EF Core),
/// allowing each module to define its own persistence provider.
/// </summary>
public static class IdentityDefaultConfiguration
{
    /// <summary>
    /// Configures default ASP.NET Core Identity options and token providers.
    /// </summary>
    /// <typeparam name="TUser">The Identity user type.</typeparam>
    /// <typeparam name="TRole">The Identity role type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDefaultIdentityConfiguration<TUser, TRole>(this IServiceCollection services)
        where TUser : class
        where TRole : class
    {
        // Register core Identity without specifying a store (EF, Mongo, etc.)
        services.AddIdentityCore<TUser>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;

            // Sign-in settings
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddRoles<TRole>();// Enables password reset, email, etc.

        return services;
    }
}
