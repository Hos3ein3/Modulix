
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ModuliX.BuildingBlocks.Extensions;

public static class HttpContextExtensions
{
    public static string? GetUserId(this HttpContext context)
        => context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public static string? GetUserEmail(this HttpContext context)
        => context.User?.FindFirst(ClaimTypes.Email)?.Value;

    public static string? GetUserRole(this HttpContext context)
        => context.User?.FindFirst(ClaimTypes.Role)?.Value;

    public static bool IsAuthenticated(this HttpContext context)
        => context.User?.Identity?.IsAuthenticated ?? false;

    public static string? GetClientIp(this HttpContext context)
        => context.Connection.RemoteIpAddress?.ToString();
}
