

using System.Globalization;
using System.Text.RegularExpressions;

namespace ModuliX.BuildingBlocks.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? value)
            => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string? value)
            => string.IsNullOrWhiteSpace(value);

        public static bool IsEmail(this string value)
            => Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static bool ContainsIgnoreCase(this string value, string compare)
            => value?.IndexOf(compare, StringComparison.OrdinalIgnoreCase) >= 0;

        public static string ToSlug(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return Regex.Replace(value.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
        }
    }
}
