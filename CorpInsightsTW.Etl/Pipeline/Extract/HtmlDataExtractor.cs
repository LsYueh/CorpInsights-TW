using System.Text.Json;
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

    public async Task<JsonDocument?> ExtractAsync(EtlContext context, CancellationToken cancellationToken, int indentLevel = 0)
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

        // TODO: ...

        throw new NotImplementedException();
    }
}