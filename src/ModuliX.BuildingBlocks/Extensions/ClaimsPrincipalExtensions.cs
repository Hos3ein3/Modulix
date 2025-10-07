

using System.Security.Claims;

namespace ModuliX.BuildingBlocks.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetClaim(this ClaimsPrincipal user, string claimType)
        => user?.FindFirst(claimType)?.Value;

    public static bool HasPermission(this ClaimsPrincipal user, string permission)
        => user?.Claims.Any(c => c.Type == "permission" && c.Value == permission) ?? false;
}
