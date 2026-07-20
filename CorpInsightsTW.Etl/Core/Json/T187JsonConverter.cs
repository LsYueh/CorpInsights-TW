using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Core.Json;

public sealed class T187JsonConverter<T> : JsonConverter<T>
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
                
                // [JsonPropertyNames] 的所有別名 (若沒有則為空清單)
                var pluralNames = namesAttr?.Names ?? [];

                // [JsonPropertyName] 的單一名稱，若沒有就降級用 C# 屬性原本的名稱
                var singleName = nameAttr?.Name ?? p.Name;

                // 聯集 (Combine + Distinct)，不重複採計且不區分大小寫
                var names = pluralNames
                    .Concat([singleName])
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                // 只有顯式掛上 [JsonRequired] 或 [JsonPropertyNames] 者，才認定為必填檢查項
                bool isRequired =
                    p.GetCustomAttribute<JsonRequiredAttribute>() is not null ||
                    namesAttr is not null;

                return new PropMeta(p, names, isRequired);
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
                var value = ReadPropertyValueSafely(ref reader, key, meta, options);
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
                    $"Property '{meta.Property.Name}' in '{typeof(T).Name}' is required, " +
                    $"but none of the aliases [{string.Join(", ", meta.Names)}] were found in the JSON.");
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

    /// <summary>
    /// 💡 處理 "" (空字串) 與 Null 的安全轉譯方法
    /// </summary>
    private static object? ReadPropertyValueSafely(
        ref Utf8JsonReader reader, string jsonKey, PropMeta meta, JsonSerializerOptions options)
    {
        var targetType = meta.Property.PropertyType;
        var propName   = meta.Property.Name;
        
        // 處理 JSON 裡面是 Null 的狀況
        if (reader.TokenType == JsonTokenType.Null)
        {
            System.Diagnostics.Debug.WriteLine($"[T187Converter Debug] 欄位 '{jsonKey}' ({propName}) 值為 Null ，降級為預設值。");
            return GetDefaultValue(targetType);
        }

        // 處理 JSON 裡面是 String (包含 "" 空字串與 "112" 字串包數字) 的狀況
        if (reader.TokenType == JsonTokenType.String)
        {
            string? strValue = reader.GetString();

            // 情況 A：吃到空字串 "" 或純空白
            if (string.IsNullOrWhiteSpace(strValue))
            {
                System.Diagnostics.Debug.WriteLine($"[T187Converter Debug] 欄位 '{jsonKey}' ({propName}) 資料為空字串，目標型別: {targetType.Name}");
                
                // 如果目標型別就是 string，直接回傳空字串或 null
                if (targetType == typeof(string)) return strValue;

                // 如果目標型別是 Nullable<T> (如 int? / decimal?)，回傳 null
                if (Nullable.GetUnderlyingType(targetType) != null) return null;

                // 如果是不可空的數值型別 (如 int / decimal)，回傳該型別的預設值 (如 0, 0.00m)
                return GetDefaultValue(targetType);
            }

            // 情況 B：目標型別剛好就是 string
            if (targetType == typeof(string))
            {
                System.Diagnostics.Debug.WriteLine($"[T187Converter Debug] 欄位 '{jsonKey}' ({propName}) 為字串: \"{strValue}\"");
                return strValue;
            }

            // 情況 C：目標型別是數值/日期/布林，但 JSON 給的是字串 (例如 "112", "1000.50")
            try
            {
                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                System.Diagnostics.Debug.WriteLine($"[T187Converter Debug] 欄位 '{jsonKey}' ({propName}) 將字串 \"{strValue}\" 轉型為: {underlyingType.Name}");
                
                // 直接手動用 Convert/TypeDescriptor 轉譯，避免 reader 再次流轉導致讀取偏移 error
                if (underlyingType == typeof(int)      &&      int.TryParse(strValue, out var  intVal)) return  intVal;
                if (underlyingType == typeof(decimal)  &&  decimal.TryParse(strValue, out var  decVal)) return  decVal;
                if (underlyingType == typeof(double)   &&   double.TryParse(strValue, out var  dblVal)) return  dblVal;
                if (underlyingType == typeof(long)     &&     long.TryParse(strValue, out var longVal)) return longVal;
                if (underlyingType == typeof(bool)     &&     bool.TryParse(strValue, out var boolVal)) return boolVal;
                if (underlyingType == typeof(DateTime) && DateTime.TryParse(strValue, out var   dtVal)) return   dtVal;

                // 其他特殊型別嘗試用 System.Convert 轉型
                return Convert.ChangeType(strValue, underlyingType);
            }
            catch (Exception ex)
            {
                throw new JsonException(
                    $"[T187Converter 轉譯失敗] 欄位 '{jsonKey}' ({propName}) 的字串值 \"{strValue}\" 無法轉為型別 {targetType.Name}。內容: {ex.Message}", ex);
            }
        }

        // 原生數字、物件或陣列等 TokenType，交給原生的 JsonSerializer 處理
        try
        {
            return JsonSerializer.Deserialize(ref reader, targetType, options);
        }
        catch (Exception ex)
        {
            throw new JsonException(
                $"[T187Converter 轉譯失敗] 處理 JSON 欄位 '{jsonKey}' -> C# 屬性 '{propName}' (型別: {targetType.Name}) 時發生錯誤。內容: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 取得非 Nullable 型別的預設值 (0, 0.0m, default(T) 等)
    /// </summary>
    private static object? GetDefaultValue(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        
        return null;
    }
}