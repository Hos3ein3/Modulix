

namespace ModuliX.BuildingBlocks.Extensions;

public static class DateTimeExtensions
{
    public static long ToUnixTimestamp(this DateTime dateTime)
        => new DateTimeOffset(dateTime).ToUnixTimeSeconds();

    public static DateTime FromUnixTimestamp(this long timestamp)
        => DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;

    public static bool IsWeekend(this DateTime date)
        => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public static bool IsToday(this DateTime date)
        => date.Date == DateTime.UtcNow.Date;

    public static string ToReadableFormat(this TimeSpan span)
        => $"{(int)span.TotalHours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
}
