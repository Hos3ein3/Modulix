
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.Extensions;

public static class ServiceCollectionExtensions
{
    public static TConfig BindConfig<TConfig>(this IServiceCollection services, IConfiguration config, string sectionName)
        where TConfig : class, new()
    {
        var settings = new TConfig();
        config.GetSection(sectionName).Bind(settings);
        services.AddSingleton(settings);
        return settings;
    }

    public static IServiceCollection AddIf<TService>(this IServiceCollection services, bool condition, Func<IServiceCollection, IServiceCollection> action)
    {
        return condition ? action(services) : services;
    }
}
