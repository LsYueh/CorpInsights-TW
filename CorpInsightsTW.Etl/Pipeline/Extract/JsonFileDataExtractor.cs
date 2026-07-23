using System.Text.Json;
using CorpInsightsTW.Core.Storage;
using CorpInsightsTW.Etl.Core.Common;

namespace CorpInsightsTW.Etl.Pipeline.Extract;

public class JsonFileDataExtractor(
    ILogger<JsonFileDataExtractor> logger,
    LocalRawDataStorage storage) : IDataExtractor
{
    private readonly ILogger<JsonFileDataExtractor> _logger = logger;
    private readonly LocalRawDataStorage _storage = storage;

    private static string GetIndent(int level) => new(' ', level * 4);

    public async Task<JsonDocument?> ExtractAsync(EtlContext context, CancellationToken cancellationToken, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        var storageContext = new StorageContext(context.ApCode, context.Status, context.Taxonomy, context.Date);
        
        string path = _storage.GetStoragePath(storageContext, indentLevel + 1);

        // 檢查檔案是否存在
        if (!_storage.Exists(storageContext, indentLevel + 1))
        {
            
            _logger.LogWarning("{Indent}⚠️ 找不到對應的原始檔案： {Path}", indent, path);
            return null;
        }

        _logger.LogInformation("{Indent}📥 檔案: {Path}", indent, path);
        
        // 透過基礎建設層的 Stream 機制唯讀開啟
        using var stream = _storage.OpenReadableStream(storageContext, indentLevel + 1);
        
        // 高效反序列化成 JsonDocument 並回傳
        return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
    }
}