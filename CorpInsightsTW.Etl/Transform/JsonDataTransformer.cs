using System.Text.Json;

namespace CorpInsightsTW.Etl.Transform;

public class JsonDataTransformer : IDataTransformer
{
    /// <summary>
    /// 將 JsonDocument 的 data 陣列攤平為 JsonElement 集合
    /// </summary>
    public IEnumerable<JsonElement> Transform(JsonDocument doc)
    {
        // 將核心陣列資料以輕量化元素 (JsonElement) 丟給後面處理
        return doc.RootElement.EnumerateArray();
    }
}