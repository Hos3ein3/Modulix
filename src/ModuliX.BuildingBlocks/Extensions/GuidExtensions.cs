
namespace ModuliX.BuildingBlocks.Extensions;

public static class GuidExtensions
{
    public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;

    public static string ToShortString(this Guid guid)
        => Convert.ToBase64String(guid.ToByteArray())
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "");
}
