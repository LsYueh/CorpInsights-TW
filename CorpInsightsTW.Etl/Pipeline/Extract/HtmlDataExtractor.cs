using System.Text.Encodings.Web;
using System.Text.Json;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Core.Storage;
using CorpInsightsTW.Etl.Core.Context;

namespace CorpInsightsTW.Etl.Pipeline.Extract;

public class HtmlDataExtractor(
    ILogger<HtmlDataExtractor> logger,
    LocalRawDataStorage storage) : IDataExtractor
{
    private readonly ILogger<HtmlDataExtractor> _logger = logger;
    private readonly LocalRawDataStorage _storage = storage;

    private static string GetIndent(int level) => new(' ', level * 4);

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 避免中文字被轉成 \uXXXX
        WriteIndented = false
    };

    public async Task<JsonDocument?> ExtractAsync(EtlContext context, CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        var storageContext = new StorageContext(context.Market, context.Type, context.Status, context.Taxonomy, context.Date);
        
        string path = _storage.GetStoragePath(storageContext, indentLevel + 1);

        // 檢查檔案是否存在
        if (!_storage.Exists(storageContext, indentLevel + 1))
        {
            _logger.LogWarning("{Indent}⚠️ 找不到對應的原始檔案： {Path}", indent, path);
            return null;
        }

        _logger.LogInformation("{Indent}📥 檔案: {Path}", indent, path);

        using var stream = _storage.OpenReadableStream(storageContext, indentLevel + 1);

        JsonDocument? jsonDocument = await HtmlDocument_ParseAsync(context, stream, ct, indentLevel + 1);

        return jsonDocument;
    }

    private async Task<JsonDocument?> HtmlDocument_ParseAsync(EtlContext context, FileStream stream, CancellationToken ct, int indentLevel = 0)
    {      
        string indent = GetIndent(indentLevel);
        
        if (stream.CanSeek) stream.Position = 0;
        
        var htmlParser = new HtmlParser();
        var document = await htmlParser.ParseDocumentAsync(stream, ct);

        var tableElements = document.QuerySelectorAll("table");

        List<Dictionary<string, string>> allRows = [];

        foreach (var element in tableElements)
        {
            // 提取符合的資料列
            List<List<string>> rowsWith9Columns = Extract9ColumnRows(element);

            // 至少要有一列 Header + 一列 Data
            if (rowsWith9Columns.Count < 2) continue;

            // 第一列拿來當 Key，其餘為 Data
            List<string> headers = rowsWith9Columns[0];
            var dataRows = rowsWith9Columns.Skip(1);

            // 組裝成 Key-Value Dictionary
            var rowObjects = ToStatementRows(context, headers, dataRows);

            _logger.LogInformation("{Indent}✅ [Parse] 解析 {Count} 列資料", indent, rowObjects.Count);

            allRows.AddRange(rowObjects);
        }

        _logger.LogInformation("{Indent}✅ [Parse] 全部解析共 {Count} 列資料", indent, allRows.Count);

        if (allRows.Count == 0) return null;

        return JsonSerializer.SerializeToDocument(allRows, jsonOptions);
    }

    /// <summary>
    /// 從指定的 HTML 元素 (Table) 中提取所有包含 9 個欄位的資料列
    /// </summary>
    private static List<List<string>> Extract9ColumnRows(IElement element)
    {
        return [.. element.QuerySelectorAll("tr")
            .Select(tr => tr.QuerySelectorAll("th, td")
                            .Select(cell => cell.TextContent.Trim())
                            .ToList())
            .Where(row => row.Count > 0)
            .Where(columns => columns.Count == 9)];
    }

    /// <summary>
    /// 將多筆資料列根據 Header 對應轉換為 Dictionary 物件清單
    /// </summary>
    private static List<Dictionary<string, string>> ToStatementRows(
        EtlContext context, List<string> headers, IEnumerable<List<string>> dataRows)
    {
        var (filingYear, filingQuarter) = context.Date.GetFilingPeriod();
        
        string exportDate = context.Date.ToMinguoDateString("yyyMMdd");
        string minguoYear = (filingYear - 1911).ToString();
        string quarter    = filingQuarter.ToString();
        
        return [.. dataRows.Select(dataRow => MapToStatementRow(
            headers, dataRow, exportDate, minguoYear, quarter
        ))];
    }

    /// <summary>
    /// 將單一資料列對應為 Dictionary
    /// </summary>
    private static Dictionary<string, string> MapToStatementRow(
        List<string> headers, List<string> dataRow,
        string exportDate, string minguoYear, string quarter)
    {
        var dict = new Dictionary<string, string> // 補上 Statement 要的基本欄位
        {
            ["出表日期"] = exportDate,
            ["年度"] = minguoYear, 
            ["季別"] = quarter
        };

        for (int i = 0; i < headers.Count; i++)
        {
            // 並處理 Header 空白與重複問題
            string rawKey = string.IsNullOrWhiteSpace(headers[i]) ? $"Column_{i + 1}" : headers[i];
            string uniqueKey = GetUniqueKey(dict, rawKey);

            // 防呆：避免 dataRow 長度不足導致 IndexOutOfRangeException
            string rawValue = i < dataRow.Count ? dataRow[i] : string.Empty;

            // 清洗數值/字串 (處理 "--"、" - " 或全形/半形符號)
            dict[uniqueKey] = CleanValue(rawValue);
        }

        return dict;
    }

    /// <summary>
    /// 防禦 Key 重複（避免 Dictionary 撞名稱）
    /// </summary>
    private static string GetUniqueKey(Dictionary<string, string> dict, string baseKey)
    {
        string uniqueKey = baseKey;
        int suffix = 1;

        while (dict.ContainsKey(uniqueKey))
        {
            uniqueKey = $"{baseKey}_{suffix++}";
        }

        return uniqueKey;
    }

    /// <summary>
    /// 清洗 MOPS/外部資料常見的特殊空值符號
    /// </summary>
    private static string CleanValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        var trimmed = value.Trim();

        // 常見代表「無資料」或「不適用」的文字
        return trimmed switch
        {
            "--" or "-" or "—" or "－" or "N/A" or "NA" or "NULL" => string.Empty,
            _ => trimmed
        };
    }
}