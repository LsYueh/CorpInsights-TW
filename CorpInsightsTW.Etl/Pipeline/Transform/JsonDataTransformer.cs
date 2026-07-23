using System.Text.Json;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;

namespace CorpInsightsTW.Etl.Pipeline.Transform;

public class JsonDataTransformer(
    ILogger<JsonDataTransformer> logger) : IDataTransformer
{
    private readonly ILogger<JsonDataTransformer > _logger = logger;

    private static string GetIndent(int level) => new(' ', level * 4);
    
    /// <summary>
    /// 將 JsonDocument 的陣列攤開, 切塊（Batching）輸出
    /// </summary>
    public IEnumerable<(IReadOnlyList<IT187Dto> Batch, int TotalCount)> Transform(
        EtlContext context, JsonDocument doc, int batchSize, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        int totalCount = doc.RootElement.GetArrayLength();

        var buffer = new List<IT187Dto>(batchSize);

        foreach (JsonElement row in doc.RootElement.EnumerateArray())
        {
            IT187Dto? dto = T187DtoFactory.MapToStrongTypeDto(context, row);
            
            if (dto != null)
            {
                // 透過介面進行防禦性檢查
                if (!dto.IsValidKey())
                {
                    _logger?.LogWarning("{Indent}⚠️ [Transform] 無效的主鍵資料，已跳過 | AP: {ApCode} | Taxonomy: {Taxonomy}",
                        indent, context.ApCode, context.Taxonomy);
                    continue;
                }

                dto.ListingStatus = context.Status.ToCode();
                buffer.Add(dto);
            }

            // 緩衝區裝滿時，立刻交付這一批
            if (buffer.Count >= batchSize)
            {
                yield return (buffer, totalCount);
                
                // 重新配置一個固定容量的 List，讓上一批的記憶體能順利交棒並被後續處理/釋放
                buffer = new List<IT187Dto>(batchSize);
            }
        }
        
        // 處理最後的殘餘尾數資料
        if (buffer.Count > 0)
        {
            yield return (buffer, totalCount);
        }
    }
}