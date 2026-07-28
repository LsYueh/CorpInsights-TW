using System.Text.Json;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;

namespace CorpInsightsTW.Etl.Pipeline.Transform;

public class JsonDataTransformer(
    ILogger<JsonDataTransformer> logger) : IDataTransformer
{
    private readonly ILogger<JsonDataTransformer> _logger = logger;

    private static string GetIndent(int level) => new(' ', level * 4);
    
    /// <summary>
    /// 將 JsonDocument 的陣列攤開, 切塊（Batching）輸出
    /// </summary>
    public IEnumerable<(IReadOnlyList<IStatementDto> Batch, int TotalCount)> Transform(
        EtlContext context, JsonDocument doc, int batchSize, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        int totalCount = doc.RootElement.GetArrayLength();

        var buffer = new List<IStatementDto>(batchSize);

        foreach (JsonElement row in doc.RootElement.EnumerateArray())
        {
            // -------------------------------------------------------------
            // 前置驗證
            // -------------------------------------------------------------
            var header = DtoFactory.ExtractHeader(row);

            if (header == null)
            {
                _logger?.LogWarning("{Indent}⚠️ [Transform] 無法提取 Header 結構，已跳過 | AP: {ApCode}",
                    indent, context.ApCode);
                continue;
            }

            // 主鍵防禦性檢查
            if (!header.IsValidKey())
            {
                _logger?.LogWarning("{Indent}⚠️ [Transform] 無效的主鍵資料，已跳過 | AP: {ApCode} | Taxonomy: {Taxonomy}",
                    indent, context.ApCode, context.Taxonomy);
                continue;
            }

            if (!IsDateValid(context, header, indentLevel))
            {
                // 日期不匹配或格式錯誤，跳過該筆 JSON
                continue;
            }

            // -------------------------------------------------------------
            // 解析成完整 DTO
            // -------------------------------------------------------------
            IStatementDto? dto = DtoFactory.ToDto(context, row);
            if (dto == null)
            {
                _logger?.LogWarning("{Indent}⚠️ [Transform] JSON 反序列化失敗，已跳過 | AP: {ApCode}",
                    indent, context.ApCode);
                continue;
            }

            // 寫入加工欄位並進入 Buffer
            dto.ListingStatus = context.Status.ToCode();
            buffer.Add(dto);

            // 緩衝區裝滿時，立刻交付這一批
            if (buffer.Count >= batchSize)
            {
                yield return (buffer, totalCount);
                
                // 重新配置一個固定容量的 List，讓上一批的記憶體能順利交棒並被後續處理/釋放
                buffer = new List<IStatementDto>(batchSize);
            }
        }
        
        // 處理最後的殘餘尾數資料
        if (buffer.Count > 0)
        {
            yield return (buffer, totalCount);
        }
    }

    private bool IsDateValid(EtlContext context, StatementHeaderDto header, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        if (string.IsNullOrWhiteSpace(header.ReportDate))
        {
            _logger.LogWarning("{Indent}⚠️ [Transform] Header 中的 ReportDate 欄位為空，已跳過", indent);
            return false;
        }

        // 清除前後空白，防止 "1100616 " 這種假性解析失敗
        string rawDateStr = header.ReportDate.Trim();

        // 解析民國年字串為 DateOnly (解析失敗代表格式不合法)
        if (!TryParseTaiwanDate(rawDateStr, out DateOnly rowDate))
        {
            _logger.LogWarning("{Indent}⚠️ [Transform] ReportDate '{ReportDate}' 無法解析為合法民國年，已跳過",
                indent, rawDateStr);
            return false;
        }

        // 比對日期
        if (rowDate != context.Date)
        {
            _logger.LogWarning(
                "{Indent}⚠️ [Transform] 資料日期不符！ JSON 解析: {RowDate:yyyy-MM-dd}, 預期 Context: {ExpectedDate:yyyy-MM-dd} | 已跳過",
                indent, rowDate, context.Date);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 將民國年字串 (如 "1100616" 或 "990101") 解析為 DateOnly
    /// </summary>
    private static bool TryParseTaiwanDate(string? minguoDateStr, out DateOnly date)
    {
        date = default;

        if (string.IsNullOrWhiteSpace(minguoDateStr))
        {
            return false;
        }

        // 清理字串前後空白
        ReadOnlySpan<char> span = minguoDateStr.AsSpan().Trim();

        // 民國年可能是 6 碼 (990101) 或 7 碼 (1100616)
        if (span.Length < 6 || span.Length > 7)
        {
            return false;
        }

        // 固定最後 4 碼為月日 (MMDD)，剩下前面 2~3 碼為年份
        int yearLength = span.Length - 4;

        if (!int.TryParse(span[..yearLength]           , out int minguoYear) ||
            !int.TryParse(span.Slice(yearLength    , 2), out int month) ||
            !int.TryParse(span.Slice(yearLength + 2, 2), out int day))
        {
            return false;
        }

        // 民國年轉西元年
        int adYear = minguoYear + 1911;

        // 用 DateOnly.TryFromDateTime 或直接 try-catch 建構 (防範 2/30 這類不合法日期)
        try
        {
            date = new DateOnly(adYear, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }
}