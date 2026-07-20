using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Core.Json;

public sealed class JsonPropertyNamesConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // 只處理有 JsonPropertyNamesAttribute 的型別上的屬性
        // Factory 層級：處理整個物件型別
        return typeToConvert.GetProperties()
            .Any(p => p.GetCustomAttribute<JsonPropertyNamesAttribute>() is not null);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(JsonPropertyNamesConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}