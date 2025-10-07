
namespace ModuliX.BuildingBlocks.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        => collection == null || !collection.Any();

    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        => source.GroupBy(keySelector).Select(g => g.First());

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int size)
    {
        var chunk = new List<T>(size);
        foreach (var element in source)
        {
            chunk.Add(element);
            if (chunk.Count == size)
            {
                yield return chunk.ToArray();
                chunk.Clear();
            }
        }
        if (chunk.Count > 0)
            yield return chunk.ToArray();
    }
}
