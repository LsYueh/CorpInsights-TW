using CorpInsightsTW.Etl.Extract;
using CorpInsightsTW.Etl.Transform;
using CorpInsightsTW.Etl.Load;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Etl.Pipeline;

public class EtlPipeline(
    ILogger<EtlPipeline> logger,
    EtlRunConfig config,
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
        T187ApCode    targetApCode   = _config.ApCode;
        ListingStatus targetStatus   = _config.Status;
        XbrlTaxonomy  targetTaxonomy = _config.Taxonomy;

        DateOnly      targetDate     = _config.Date;

        var statusToFetch = targetStatus == ListingStatus.All
            ? Enum.GetValues<ListingStatus>().Where(m => m != ListingStatus.All)
            : [targetStatus];

        var taxonomiesToFetch = targetTaxonomy == XbrlTaxonomy.All
            ? Enum.GetValues<XbrlTaxonomy>().Where(t => t != XbrlTaxonomy.All)
            : [targetTaxonomy];

        var reportsToFetch = targetApCode == T187ApCode.All
            ? Enum.GetValues<T187ApCode>().Where(r => r != T187ApCode.All)
            : [targetApCode];

        foreach (var taxonomy in taxonomiesToFetch)
        {
            foreach (var status in statusToFetch)
            {
                foreach (var apCode in reportsToFetch)
                {
                    _logger.LogInformation("🏁 [Pipeline] 管線目標: {ApCode}_{Status}_{Taxonomy} (日期: {Date})", 
                        apCode, status, taxonomy, targetDate.ToString("yyyyMMdd"));
                    
                    await ExecutePipelineStepAsync(apCode, status, taxonomy, targetDate, cancellationToken);
                }
            }
        }

        _logger.LogInformation("✅ [Pipeline] 全線加工完成！");
    }

    /// <summary>
    /// 單一規格組的 ETL 處理
    /// </summary>
    private async Task ExecutePipelineStepAsync (
        T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly date,
        CancellationToken cancellationToken)
    {
        try
        {
            // 📥 1. Extract
            _logger.LogDebug("📥 [Pipeline] 開始擷取 (Extract)...");
            using var rawDoc = await _extractor.ExtractAsync(apCode, status, taxonomy, date, cancellationToken);
            if (rawDoc == null)
            {
                _logger.LogWarning("⏹️ [Pipeline] {ApCode}_{Status}_{Taxonomy} ({Date}) 擷取階段未取得資料，管線提前中止。", 
                    apCode, status, taxonomy, date.ToString("yyyyMMdd"));
                return;
            }

            // 🔄 2. Transform
            _logger.LogDebug("🔄 [Pipeline] 開始轉換 (Transform)...");
            var jsonBatches = _transformer.Transform(rawDoc);

            // 💾 3. Load
            _logger.LogDebug("💾 [Pipeline] 開始載入 (Load)...");
            foreach (var (batch, totalCount) in jsonBatches)
            {
                await _loader.LoadAsync(batch, totalCount, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ [Pipeline] {ApCode}_{Status}_{Taxonomy} ({Date}) 處理時發生未預期異常！", 
                apCode.ToCode(), status.ToCode(), taxonomy.ToCode(), date.ToString("yyyyMMdd"));
            throw;
        }

    }
}