
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModuliX.BuildingBlocks.Middlewares;

/// <summary>
/// Middleware that enables culture detection and resource localization.
/// </summary>
public static class LocalizationMiddleware
{
    /// <summary>
    /// Uses the configured localization middleware pipeline.
    /// </summary>
    public static IApplicationBuilder UseDefaultLocalization(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(options.Value);
        return app;
    }
}
