using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Core.Json;

public sealed class T187JsonConverterFactory : JsonConverterFactory
{
    // Thread-Safe 快取， Reflection 操作只執行一次
    private static readonly ConcurrentDictionary<Type, bool> _cache = new();

    public override bool CanConvert(Type typeToConvert)
    {
        // 排除: 基本型別、Enum、字串
        if (typeToConvert.IsPrimitive || typeToConvert.IsEnum || typeToConvert == typeof(string))
            return false;

        // 排除: 集合/陣列型別 (List, Dictionary, Array 等)，避免對集合進行屬性掃描
        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(typeToConvert))
            return false;

        // 排除: System / Microsoft 內建組件類別
        var asmName = typeToConvert.Assembly.FullName;
        if (asmName != null && 
           (asmName.StartsWith("System"   , StringComparison.Ordinal) ||
            asmName.StartsWith("Microsoft", StringComparison.Ordinal)))
        {
            return false;
        }
        
        // 使用快取檢查，避免重複觸發 GetProperties Reflection
        return _cache.GetOrAdd(typeToConvert, t =>
        {
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Any(p => p.GetCustomAttribute<JsonPropertyNamesAttribute>() is not null ||
                          p.GetCustomAttribute<JsonPropertyNameAttribute>()  is not null ||
                          p.GetCustomAttribute<JsonRequiredAttribute>()      is not null);
        });
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(T187JsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}