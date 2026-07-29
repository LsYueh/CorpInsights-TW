using System.Text.Encodings.Web;
using System.Text.Json;
using AngleSharp.Parser.Html;
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

        JsonDocument? jsonDocument = await HtmlDocument_ParseAsync(stream, ct, indentLevel + 1);

        // if (jsonDocument != null)
        // {
        //     // 1. 使用格式化選項讓 JSON 印出來帶有縮排排版
        //     var prettyOptions = new JsonSerializerOptions
        //     {
        //         WriteIndented = true,
        //         Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // 避免中文變成 \uXXXX
        //     };

        //     string jsonString = JsonSerializer.Serialize(jsonDocument.RootElement, prettyOptions);

        //     _logger.LogInformation("{Indent}📄 解析出的 JSON 內容:\n{Json}", indent, jsonString);
        // }

        return jsonDocument;
    }

    private async Task<JsonDocument?> HtmlDocument_ParseAsync(FileStream stream, CancellationToken ct, int indentLevel = 0)
    {      
        string indent = GetIndent(indentLevel);
        
        if (stream.CanSeek) stream.Position = 0;
        
        var htmlParser = new HtmlParser();
        var document = await htmlParser.ParseAsync(stream, ct);

        return null;

        // var allRows = document.QuerySelectorAll("table tr");

        // List<List<string>> allTableData = [.. allRows
        //     .Select(tr => tr.QuerySelectorAll("th, td")
        //                     .Select(cell => cell.TextContent.Trim())
        //                     .ToList())
        //     .Where(row => row.Count > 0)
        //     .Where(columns => columns.Count == 9)];

        // if (allTableData.Count == 0) return null;

        // _logger.LogInformation("{Indent}✅ [Parse] 解析 {Count} 列資料", indent, allTableData.Count);

        // return JsonSerializer.SerializeToDocument(allTableData, jsonOptions);
    }
}