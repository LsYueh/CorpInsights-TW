using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Core.Json;

public sealed class JsonPropertyNamesConverter<T> : JsonConverter<T>
{
    // 每個屬性的元資料：PropertyInfo + 對應的名稱清單
    private sealed record PropMeta(
        PropertyInfo Property,
        string[] Names,
        bool IsRequired
    );

    private static readonly bool _hasParameterlessCtor =
        typeof(T).GetConstructor(Type.EmptyTypes) is not null;

    private static readonly PropMeta[] _metas = BuildMetas();
    private static readonly Dictionary<string, PropMeta> _lookup = BuildLookup();

    private static PropMeta[] BuildMetas()
    {
        return [.. typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite) // 確保屬性是可以寫入的
            .Select(p =>
            {
                var namesAttr = p.GetCustomAttribute<JsonPropertyNamesAttribute>();
                var nameAttr  = p.GetCustomAttribute<JsonPropertyNameAttribute>();
                var name      = nameAttr?.Name ?? p.Name;

                var names = namesAttr?.Names ?? [name];

                // 只有顯式掛上 [JsonRequired] 或 [JsonPropertyNames] 者，才認定為必填檢查項
                bool isRequired =
                    p.GetCustomAttribute<JsonRequiredAttribute>() is not null ||
                    p.GetCustomAttribute<JsonPropertyNamesAttribute>() is not null;

                return new PropMeta(p, names, isRequired);  // PropMeta 加一個 IsRequired 欄位
            })];
    }

    private static Dictionary<string, PropMeta> BuildLookup() =>
        _metas
            .SelectMany(m => m.Names.Select(n => (n, m)))
            .ToDictionary(x => x.n, x => x.m, StringComparer.OrdinalIgnoreCase);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject.");

        // 確保原本寫在 DTO 內部的預設值 (Default Value) 正常生效
        var instance = _hasParameterlessCtor
            ? Activator.CreateInstance<T>()
            : (T)RuntimeHelpers.GetUninitializedObject(typeof(T));

        var filled = new HashSet<PropMeta>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Expected PropertyName.");

            var key = reader.GetString()!;
            reader.Read(); // move to value

            if (_lookup.TryGetValue(key, out var meta))
            {
                var value = JsonSerializer.Deserialize(ref reader, meta.Property.PropertyType, options);
                meta.Property.SetValue(instance, value);
                filled.Add(meta);
            }
            else
            {
                reader.Skip();
            }
        }

        // 多別名 / 必填項防衛檢查：如果完全沒出現在 JSON 裡就 throw JsonException
        foreach (var meta in _metas)
        {
            if (meta.IsRequired && !filled.Contains(meta))
                throw new JsonException(
                    $"Property '{meta.Property.Name}' is required but none of the aliases " +
                    $"[{string.Join(", ", meta.Names)}] were found in the JSON.");
        }

        return instance;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var meta in _metas)
        {
            var propValue = meta.Property.GetValue(value);
            writer.WritePropertyName(meta.Names[0]); // 永遠用第一個名稱
            JsonSerializer.Serialize(writer, propValue, meta.Property.PropertyType, options);
        }

        writer.WriteEndObject();
    }
}