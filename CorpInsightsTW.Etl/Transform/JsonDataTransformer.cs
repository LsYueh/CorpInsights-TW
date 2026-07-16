using System.Text.Json;

namespace CorpInsightsTW.Etl.Transform;

public class JsonDataTransformer : IDataTransformer
{
    /// <summary>
    /// 將 JsonDocument 的陣列攤, 切塊（Batching）輸出
    /// </summary>
    public IEnumerable<(IReadOnlyList<JsonElement> Batch, int TotalCount)> Transform(JsonDocument doc, int batchSize, int indentLevel = 0)
    {
        int totalCount = doc.RootElement.GetArrayLength();

        var buffer = new List<JsonElement>(batchSize);

        foreach (JsonElement row in doc.RootElement.EnumerateArray())
        {
            buffer.Add(row);

            // 緩衝區裝滿時，立刻交付這一批
            if (buffer.Count >= batchSize)
            {
                yield return (buffer, totalCount);
                
                // 重新配置一個固定容量的 List，讓上一批的記憶體能順利交棒並被後續處理/釋放
                buffer = new List<JsonElement>(batchSize);
            }
        }
        
        // 處理最後的殘餘尾數資料
        if (buffer.Count > 0)
        {
            yield return (buffer, totalCount);
        }
    }
}