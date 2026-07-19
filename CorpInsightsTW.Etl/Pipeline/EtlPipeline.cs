using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Pipeline.Extract;
using CorpInsightsTW.Etl.Pipeline.Load;
using CorpInsightsTW.Etl.Pipeline.Transform;

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

    private static string GetIndent(int level) => new(' ', level * 4);

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

        _logger.LogInformation("🏁 [Pipeline] 開始執行批次排程...");

        foreach (var taxonomy in taxonomiesToFetch)
        {
            foreach (var status in statusToFetch)
            {
                foreach (var apCode in reportsToFetch)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    var context = new EtlContext(apCode, status, taxonomy, targetDate);
                    await ExecutePipelineStepAsync(context, cancellationToken, 1);
                }
            }
        }

        _logger.LogInformation("✅ [Pipeline] 全線加工完成！");
    }

    /// <summary>
    /// 單一規格組的 ETL 處理
    /// </summary>
    private async Task ExecutePipelineStepAsync (EtlContext context, CancellationToken cancellationToken, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        string tag = $"{context.ApCode.ToCode()}_{context.Status.ToCode()}_{context.Taxonomy.ToCode()}";
        string title = $"{context.Status.ToDisplay()} {context.ApCode.ToDisplay()} - {context.Taxonomy.ToDisplay()}";
        string message = $"[{context.Date:yyyyMMdd}] {tag} ({title})";

        _logger.LogInformation("{Indent}🏁 [Pipeline] 目標: {Message}", indent, message);

        try
        {
            // 📥 1. Extract
            _logger.LogDebug("{Indent}📥 [Pipeline] 開始擷取 (Extract)...", indent);
            using var rawDoc = await _extractor.ExtractAsync(context, cancellationToken, indentLevel + 1);
            if (rawDoc == null)
            {
                _logger.LogWarning("{Indent}⏹️ [Pipeline] {Message} 擷取階段未取得資料，管線提前中止。", indent, message);
                return;
            }

            // 🔄 2. Transform
            _logger.LogDebug("{Indent}🔄 [Pipeline] 開始轉換 (Transform)...", indent);

            int targetBatchSize = 200;
            var t187Batches = _transformer.Transform(context, rawDoc, targetBatchSize, indentLevel + 1);

            // 💾 3. Load
            _logger.LogDebug("{Indent}💾 [Pipeline] 開始載入 (Load)...", indent);

            int fileTotalCount = 0;
            foreach (var (batch, totalCount) in t187Batches)
            {
                fileTotalCount = totalCount;
                await _loader.LoadAsync(context, batch, totalCount, cancellationToken, indentLevel + 1);
            }

            _logger.LogInformation("{Indent}✅ [Pipeline] 完畢，共處理 {Total} 筆。", indent, fileTotalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Indent}❌ [Pipeline] {Message} 處理時發生未預期異常！", indent, message);
            throw;
        }
    }
}