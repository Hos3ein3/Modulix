
using System.ComponentModel;

namespace ModuliX.BuildingBlocks.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = (DescriptionAttribute?)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
        return attr?.Description ?? value.ToString();
    }

    /// <summary>
    /// Gets a dictionary of all enum values and their descriptions.
    /// </summary>
    public static Dictionary<int, string> GetEnumDescriptions<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<Enum>()
            .ToDictionary(e => Convert.ToInt32(e), e => e.GetDescription());
    }

    /// <summary>
    /// Parses a string or numeric value into an enum safely.
    /// Returns default(TEnum) if invalid.
    /// </summary>
    public static TEnum ParseEnum<TEnum>(this string value, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(value, ignoreCase, out var result))
            return result;

        return default;
    }

    /// <summary>
    /// Tries to parse a string to an enum and returns a bool indicating success.
    /// </summary>
    public static bool TryParseEnum<TEnum>(this string value, out TEnum result, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase, out result);
    }

    /// <summary>
    /// Returns the next enum value (wraps around if at the end).
    /// </summary>
    public static TEnum Next<TEnum>(this TEnum value) where TEnum : Enum
    {
        var values = (TEnum[])Enum.GetValues(typeof(TEnum));
        var index = Array.IndexOf(values, value);
        return values[(index + 1) % values.Length];
    }

    /// <summary>
    /// Returns the previous enum value (wraps around if at the beginning).
    /// </summary>
    public static TEnum Previous<TEnum>(this TEnum value) where TEnum : Enum
    {
        var values = (TEnum[])Enum.GetValues(typeof(TEnum));
        var index = Array.IndexOf(values, value);
        return values[(index - 1 + values.Length) % values.Length];
    }

    /// <summary>
    /// Converts an enum to a list of (int, name, description) tuples.
    /// Useful for dropdowns or API output.
    /// </summary>
    public static IEnumerable<(int Value, string Name, string Description)> ToList<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => (Convert.ToInt32(e), e.ToString(), (e as Enum).GetDescription()));
    }

    /// <summary>
    /// Gets a random value from an enum.
    /// </summary>
    public static TEnum RandomValue<TEnum>() where TEnum : Enum
    {
        var values = Enum.GetValues(typeof(TEnum));
        var index = new Random().Next(values.Length);
        return (TEnum)values.GetValue(index)!;
    }

    /// <summary>
    /// Checks if a flag enum contains a specific flag value.
    /// </summary>
    public static bool HasFlagValue<TEnum>(this TEnum value, TEnum flag) where TEnum : Enum
    {
        var intValue = Convert.ToInt64(value);
        var intFlag = Convert.ToInt64(flag);
        return (intValue & intFlag) == intFlag;
    }

    /// <summary>
    /// Adds a flag to a flag-based enum.
    /// </summary>
    public static TEnum AddFlag<TEnum>(this TEnum value, TEnum flag) where TEnum : Enum
    {
        var result = Convert.ToInt64(value) | Convert.ToInt64(flag);
        return (TEnum)Enum.ToObject(typeof(TEnum), result);
    }

    /// <summary>
    /// Removes a flag from a flag-based enum.
    /// </summary>
    public static TEnum RemoveFlag<TEnum>(this TEnum value, TEnum flag) where TEnum : Enum
    {
        var result = Convert.ToInt64(value) & ~Convert.ToInt64(flag);
        return (TEnum)Enum.ToObject(typeof(TEnum), result);
    }

    /// <summary>
    /// Converts an enum value to its underlying integer type.
    /// </summary>
    public static int ToInt(this Enum value) => Convert.ToInt32(value);

    /// <summary>
    /// Converts an enum value to its string name.
    /// </summary>
    public static string ToName(this Enum value) => value.ToString();

    /// <summary>
    /// Converts an integer to an enum value safely.
    /// </summary>
    public static TEnum FromInt<TEnum>(this int value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value)
            ? (TEnum)Enum.ToObject(typeof(TEnum), value)
            : default!;
    }

    /// <summary>
    /// Checks if a numeric value is defined in the enum.
    /// </summary>
    public static bool IsDefined<TEnum>(this int value) where TEnum : Enum
        => Enum.IsDefined(typeof(TEnum), value);

}
