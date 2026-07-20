using System.Text.Json;
using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Core.Json;

/// <summary>
/// 針對政府公開資料容錯設計的數字轉換器
/// 遇到空字串 ""、純空白 "   " 或 null 時，會自動防禦並回傳 0，避免反序列化崩潰
/// </summary>
public class SmartDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 1. 如果 JSON 原生就是數字 (e.g. 100 或 100.5)
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal();
        }

        // 2. 如果 JSON 包成了字串 (e.g. "100" 或 "")
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();

            // 💡 關鍵防守：如果是 null、空字串 "" 或全空白 "  "，直接容錯回傳 0
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return 0m;
            }

            // 嘗試解析真正的數字字串 (例如 "123.45")
            if (decimal.TryParse(stringValue, out decimal result))
            {
                return result;
            }
        }

        // 3. 遇到 null token 時預設給 0
        if (reader.TokenType == JsonTokenType.Null)
        {
            return 0m;
        }

        // 若真的無法解析 (如 "N/A")，亦可回傳 0，或拋出例外
        return 0m;
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}