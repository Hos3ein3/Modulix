
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModuliX.BuildingBlocks.Extensions;

/// <summary>
/// Helpful extension methods for working with Newtonsoft.Json.
/// Supports safe serialization, deep clone, dynamic parsing, and property manipulation.
/// </summary>
public static class NewtonsoftJsonExtensions
{
    private static readonly JsonSerializerSettings DefaultSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.None
    };

    /// <summary>
    /// Serializes an object to a JSON string using Newtonsoft.Json.
    /// </summary>
    public static string ToJson(this object obj, bool indented = false, JsonSerializerSettings? settings = null)
    {
        var options = settings ?? DefaultSettings;
        options.Formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(obj, options);
    }

    /// <summary>
    /// Deserializes a JSON string into a typed object safely.
    /// Returns default(T) if the string is null or invalid.
    /// </summary>
    public static T? FromJson<T>(this string? json, JsonSerializerSettings? settings = null)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        try
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSettings);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Performs a deep clone by serializing and deserializing the object.
    /// </summary>
    public static T? DeepClone<T>(this T source)
    {
        if (source == null)
            return default;
        var json = JsonConvert.SerializeObject(source, DefaultSettings);
        return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
    }

    /// <summary>
    /// Tries to get a property value from a JObject.
    /// Returns default if property doesn't exist or conversion fails.
    /// </summary>
    public static T? GetPropertyValue<T>(this JObject jObject, string propertyName)
    {
        if (jObject.TryGetValue(propertyName, StringComparison.OrdinalIgnoreCase, out var token))
        {
            try
            {
                return token.ToObject<T>();
            }
            catch
            {
                return default;
            }
        }
        return default;
    }

    /// <summary>
    /// Adds or updates a property in a JObject.
    /// </summary>
    public static JObject SetPropertyValue(this JObject jObject, string propertyName, object? value)
    {
        jObject[propertyName] = value != null ? JToken.FromObject(value) : JValue.CreateNull();
        return jObject;
    }

    /// <summary>
    /// Removes a property from a JObject if it exists.
    /// </summary>
    public static JObject RemoveProperty(this JObject jObject, string propertyName)
    {
        if (jObject.ContainsKey(propertyName))
            jObject.Remove(propertyName);
        return jObject;
    }

    /// <summary>
    /// Converts any object to JObject (for dynamic access).
    /// </summary>
    public static JObject ToJObject(this object obj)
    {
        if (obj is JObject jObj)
            return jObj;

        return JObject.FromObject(obj, JsonSerializer.Create(DefaultSettings));
    }

    /// <summary>
    /// Merges another JSON object or string into an existing JObject.
    /// </summary>
    public static JObject MergeWith(this JObject target, object source)
    {
        var sourceObj = source switch
        {
            JObject jObj => jObj,
            string json => JObject.Parse(json),
            _ => JObject.FromObject(source)
        };

        target.Merge(sourceObj, new JsonMergeSettings
        {
            MergeArrayHandling = MergeArrayHandling.Union,
            MergeNullValueHandling = MergeNullValueHandling.Ignore
        });

        return target;
    }

    /// <summary>
    /// Flattens nested JSON objects into a simple dictionary.
    /// </summary>
    public static Dictionary<string, string?> Flatten(this JObject jObject, string? parentPath = null)
    {
        var dict = new Dictionary<string, string?>();
        foreach (var prop in jObject.Properties())
        {
            var path = string.IsNullOrEmpty(parentPath) ? prop.Name : $"{parentPath}.{prop.Name}";
            if (prop.Value is JObject nested)
                foreach (var kv in nested.Flatten(path))
                    dict[kv.Key] = kv.Value;
            else
                dict[path] = prop.Value?.ToString();
        }
        return dict;
    }

    /// <summary>
    /// Converts a JSON array into a strongly typed list.
    /// </summary>
    public static List<T> ToList<T>(this JArray array)
        => array.ToObject<List<T>>() ?? new List<T>();
}
