using System.Text.Json;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;

namespace CorpInsightsTW.Etl.Pipeline.Transform;

public class JsonDataTransformer : IDataTransformer
{
    /// <summary>
    /// 將 JsonDocument 的陣列攤開, 切塊（Batching）輸出
    /// </summary>
    public IEnumerable<(IReadOnlyList<IT187RawDto> Batch, int TotalCount)> Transform(
        EtlContext context, JsonDocument doc, int batchSize, int indentLevel = 0)
    {
        int totalCount = doc.RootElement.GetArrayLength();

        var buffer = new List<IT187RawDto>(batchSize);

        foreach (JsonElement row in doc.RootElement.EnumerateArray())
        {
            IT187RawDto? dto = T187DtoFactory.MapToStrongTypeDto(context, row);
            
            if (dto != null)
            {
                buffer.Add(dto);
            }

            // 緩衝區裝滿時，立刻交付這一批
            if (buffer.Count >= batchSize)
            {
                yield return (buffer, totalCount);
                
                // 重新配置一個固定容量的 List，讓上一批的記憶體能順利交棒並被後續處理/釋放
                buffer = new List<IT187RawDto>(batchSize);
            }
        }
        
        // 處理最後的殘餘尾數資料
        if (buffer.Count > 0)
        {
            yield return (buffer, totalCount);
        }
    }
}