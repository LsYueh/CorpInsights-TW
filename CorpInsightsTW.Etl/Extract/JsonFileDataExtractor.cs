using System.Text.Json;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Infrastructure.Storage;

namespace CorpInsightsTW.Etl.Extract;

public class JsonFileDataExtractor(ILogger<JsonFileDataExtractor> logger, EtlRunConfig config) : IDataExtractor
{
    private readonly ILogger<JsonFileDataExtractor> _logger = logger;
    private readonly EtlRunConfig _config = config;

    public async Task<JsonDocument?> ExtractAsync(CancellationToken cancellationToken)
    {
        // 🎯 防禦性檢查今日檔案是否存在
        // 備註：若 LocalRawDataStorage 目前只支援今日，未來可擴充支援帶入 DateOnly 參數的重載
        if (!LocalRawDataStorage.Exists(_config.ApCode, _config.Status, _config.Taxonomy))
        {
            _logger.LogWarning("⚠️ 找不到對應的原始落地檔案：{ApCode}_{Status}_{Taxonomy}", _config.ApCode, _config.Status, _config.Taxonomy);
            return null;
        }

        _logger.LogInformation("📥 開啟本地原始檔案流...");
        
        // 透過基礎建設層的 Stream 機制唯讀開啟
        using var stream = LocalRawDataStorage.OpenReadableStream(_config.ApCode, _config.Status, _config.Taxonomy);
        
        // 高效反序列化成 JsonDocument 並回傳
        return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
    }
}