using System.Text.Json;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Infrastructure.Storage;

namespace CorpInsightsTW.Etl.Extract;

public class JsonFileDataExtractor(
    ILogger<JsonFileDataExtractor> logger,
    LocalRawDataStorage storage) : IDataExtractor
{
    private readonly ILogger<JsonFileDataExtractor> _logger = logger;
    private readonly LocalRawDataStorage _storage = storage;

    public async Task<JsonDocument?> ExtractAsync(
        T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly date,
        CancellationToken cancellationToken)
    {
        // 檢查檔案是否存在
        if (!_storage.Exists(apCode, status, taxonomy, date))
        {
            string path = _storage.GetStoragePath(apCode, status, taxonomy, date);
            _logger.LogWarning("⚠️ 找不到對應的原始檔案： {Path}", path);
            return null;
        }

        _logger.LogInformation("📥 開啟本地原始檔案流...");
        
        // 透過基礎建設層的 Stream 機制唯讀開啟
        using var stream = _storage.OpenReadableStream(apCode, status, taxonomy, date);
        
        // 高效反序列化成 JsonDocument 並回傳
        return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
    }
}