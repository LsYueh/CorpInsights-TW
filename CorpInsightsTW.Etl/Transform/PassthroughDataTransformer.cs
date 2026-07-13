using System.Text.Json;

namespace CorpInsightsTW.Etl.Transform;

public class PassthroughDataTransformer(ILogger<PassthroughDataTransformer> logger) : IDataTransformer
{
    private readonly ILogger<PassthroughDataTransformer> _logger = logger;

    public JsonDocument Transform(JsonDocument source)
    {
        _logger.LogInformation("🔄 執行資料轉換 (目前為直通模式)...");
        
        // (目前先不做轉換)
        return source;
    }
}