
using System.Text.Json;

namespace ModuliX.BuildingBlocks.Extensions;

public static class ObjectExtensions
{
    public static string ToJson(this object obj, bool indented = false)
        => JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = indented });

    public static T? Clone<T>(this T source)
    {
        var json = JsonSerializer.Serialize(source);
        return JsonSerializer.Deserialize<T>(json);
    }
}
