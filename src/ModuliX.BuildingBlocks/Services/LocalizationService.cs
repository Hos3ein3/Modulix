
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.Services;
/// <summary>
/// Configures localization and resource file support for multi-language modules.
/// </summary>
public static class LocalizationService
{
    /// <summary>
    /// Registers default localization services for .resx resource files.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="resourcesPath">Path to resource files (default: "Resources").</param>
    /// <param name="supportedCultures">Optional custom list of supported cultures.</param>
    /// <param name="defaultCulture">Default culture (e.g., "en").</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDefaultLocalizationServices(
        this IServiceCollection services,
        string resourcesPath = "Resources",
        IEnumerable<string>? supportedCultures = null,
        string defaultCulture = "en")
    {
        services.AddLocalization(options => options.ResourcesPath = resourcesPath);

        var cultures = supportedCultures is not null
            ? new List<CultureInfo>()
            : new List<CultureInfo>
            {
                    new CultureInfo("en"),
                    new CultureInfo("it"),
                    new CultureInfo("fa"),
                    new CultureInfo("fr")
            };

        if (supportedCultures is not null)
        {
            foreach (var code in supportedCultures)
                cultures.Add(new CultureInfo(code));
        }

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;

            // Order of culture detection:
            options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            options.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());
            options.RequestCultureProviders.Insert(2, new AcceptLanguageHeaderRequestCultureProvider());
        });

        return services;
    }
}
