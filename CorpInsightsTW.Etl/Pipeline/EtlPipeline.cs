using CorpInsightsTW.Etl.Extract;
using CorpInsightsTW.Etl.Transform;
using CorpInsightsTW.Etl.Load;

namespace CorpInsightsTW.Etl.Pipeline;

public class EtlPipeline(
    ILogger<EtlPipeline> logger,
    EtlRunConfig config, // 🎯 雖然 pipeline 主要控制流程，但也能注入來印 Log
    IDataExtractor extractor,
    IDataTransformer transformer,
    IDataLoader loader)
{
    private readonly ILogger<EtlPipeline> _logger = logger;
    private readonly EtlRunConfig _config = config;
    private readonly IDataExtractor _extractor = extractor;
    private readonly IDataTransformer _transformer = transformer;
    private readonly IDataLoader _loader = loader;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🏁 [Pipeline] 管線目標: {ApCode}_{Status}_{Taxonomy} (日期: {Date})", 
            _config.ApCode, _config.Status, _config.Taxonomy, _config.Date.ToString("yyyyMMdd"));

        // 📥 1. Extract
        using var rawDoc = await _extractor.ExtractAsync(cancellationToken);
        if (rawDoc == null)
        {
            _logger.LogWarning("⏹️ [Pipeline] 擷取階段未取得資料，管線提前中止。");
            return;
        }

        // 🔄 2. Transform (暫時直通)
        using var transformedDoc = _transformer.Transform(rawDoc);

        // 💾 3. Load (暫時印到 Console)
        await _loader.LoadAsync(transformedDoc, cancellationToken);

        _logger.LogInformation("✅ [Pipeline] 全線加工完成！");
    }
}