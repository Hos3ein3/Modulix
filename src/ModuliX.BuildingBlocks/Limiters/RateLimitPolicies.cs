
using System;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace ModuliX.BuildingBlocks.Limiters;
/// <summary>
/// Provides predefined and dynamic rate limit policies for APIs.
/// Can be used globally or per-endpoint.
/// </summary>
public static class RateLimitPolicies
{

    public static readonly string[] RateLimiterPolicyNames = new[] { "OneRequestPer5Min" };


    /// <summary>
    /// Registers predefined and dynamic rate limiters.
    /// </summary>
    public static IServiceCollection AddDefaultRateLimitPolicies(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // --- Default predefined limiters ---
            options.AddFixedWindowLimiter("OneRequestPer5Min", limiterOptions =>
            {
                limiterOptions.PermitLimit = 1;
                limiterOptions.Window = TimeSpan.FromMinutes(5);
                limiterOptions.QueueLimit = 0;
            });

            options.AddFixedWindowLimiter("OneRequestPer30Min", limiterOptions =>
            {
                limiterOptions.PermitLimit = 1;
                limiterOptions.Window = TimeSpan.FromMinutes(30);
                limiterOptions.QueueLimit = 0;
            });

            options.AddFixedWindowLimiter("OneRequestPerHour", limiterOptions =>
            {
                limiterOptions.PermitLimit = 1;
                limiterOptions.Window = TimeSpan.FromHours(1);
                limiterOptions.QueueLimit = 0;
            });

            // --- Token Bucket example ---
            options.AddTokenBucketLimiter("TenPerMinute", limiterOptions =>
            {
                limiterOptions.TokenLimit = 10;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
                limiterOptions.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
                limiterOptions.TokensPerPeriod = 10;
            });
        });

        return services;
    }

    /// <summary>
    /// Dynamically creates a Fixed Window rate limiter based on user input.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="policyName">Unique name for the policy.</param>
    /// <param name="permitLimit">How many requests are allowed per window.</param>
    /// <param name="window">The time window (e.g., 5min, 1h).</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCustomFixedWindowLimiter(
        this IServiceCollection services,
        string policyName,
        int permitLimit,
        TimeSpan window)
    {
        services.AddRateLimiter(options =>

            options.AddFixedWindowLimiter(policyName, limiterOptions =>
            {
                limiterOptions.PermitLimit = permitLimit;
                limiterOptions.Window = window;
                limiterOptions.QueueLimit = 0;
            })
        );

        return services;
    }

    /// <summary>
    /// Dynamically creates a Sliding Window rate limiter.
    /// </summary>
    public static IServiceCollection AddCustomSlidingWindowLimiter(
        this IServiceCollection services,
        string policyName,
        int permitLimit,
        TimeSpan window,
        int segments = 2)
    {
        services.AddRateLimiter(options =>

            options.AddSlidingWindowLimiter(policyName, limiterOptions =>
            {
                limiterOptions.PermitLimit = permitLimit;
                limiterOptions.Window = window;
                limiterOptions.SegmentsPerWindow = segments;
                limiterOptions.QueueLimit = 0;
            })
        );

        return services;
    }

    /// <summary>
    /// Dynamically creates a Token Bucket rate limiter (useful for burst traffic).
    /// </summary>
    public static IServiceCollection AddCustomTokenBucketLimiter(
        this IServiceCollection services,
        string policyName,
        int tokenLimit,
        int tokensPerPeriod,
        TimeSpan replenishmentPeriod)
    {
        services.AddRateLimiter(options =>

            options.AddTokenBucketLimiter(policyName, limiterOptions =>
            {
                limiterOptions.TokenLimit = tokenLimit;
                limiterOptions.TokensPerPeriod = tokensPerPeriod;
                limiterOptions.ReplenishmentPeriod = replenishmentPeriod;
                limiterOptions.QueueLimit = 0;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            })
        );

        return services;
    }
}
