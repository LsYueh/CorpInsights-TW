using System.Text.Json;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Pipeline.Extract;
using CorpInsightsTW.Etl.Pipeline.Load;
using CorpInsightsTW.Etl.Pipeline.Transform;

namespace CorpInsightsTW.Etl.Pipeline;

public class EtlPipeline(
    ILogger<EtlPipeline> logger,
    RuntimeConfig config,
    IDataExtractor extractor,
    IDataTransformer transformer,
    IDataLoader loader)
{
    private readonly ILogger<EtlPipeline> _logger = logger;
    private readonly RuntimeConfig _config = config;

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

        _logger.LogInformation("✅ [Pipeline] 批次排程結束");
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

        int currentBatchIndex = 0; 
        int fileTotalCount = 0;
        
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

            foreach (var (batch, totalCount) in t187Batches)
            {
                currentBatchIndex++;
                fileTotalCount = totalCount;
                await _loader.LoadAsync(context, batch, totalCount, cancellationToken, indentLevel + 1);
            }

            _logger.LogInformation("{Indent}✅ [Pipeline] 完畢，共處理 {Total} 筆。", indent, fileTotalCount);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogCritical(jsonEx, "{Indent}💥 [Pipeline] {Tag} JSON 解析嚴重失敗！",
                indent, tag);
            _logger.LogCritical(jsonEx, "{Indent}{Indent}👉 錯誤原因: {ExMessage}",
                indent, indent, jsonEx.Message);
            _logger.LogCritical(jsonEx, "{Indent}{Indent}👉 JSON 錯誤位置: 行號 {LineNumber} | 該行字元位置 {BytePositionInLine}",
                indent, indent, jsonEx.LineNumber, jsonEx.BytePositionInLine);
            _logger.LogCritical(jsonEx, "{Indent}{Indent}👉 偵錯提示: 請檢查 DTO 定義的 [JsonPropertyName] 是否與政府最新欄位名稱一致，或數值型別是否不符。",
                indent, indent);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Indent}❌ [Pipeline] {Tag} 處理時發生未預期異常！\n",
                indent, tag);
            _logger.LogError(ex, "{Indent}{Indent}👉 執行上下文: 報表={ApCode}, 市場狀態={Status}, 分類={Taxonomy}, 日期={Date}\n",
                indent, indent, context.ApCode, context.Status, context.Taxonomy, context.Date);
            _logger.LogError(ex, "{Indent}{Indent}👉 當前進度: 已成功處理到第 {BatchIdx} 批次 (總共約 {Total} 筆)\n",
                indent, indent, currentBatchIndex + 1, fileTotalCount);
            _logger.LogError(ex, "{Indent}{Indent}👉 異常類型: {ExType} | 訊息: {ExMessage}", 
                indent, indent, ex.GetType().Name, ex.Message);
            throw;
        }
    }
}