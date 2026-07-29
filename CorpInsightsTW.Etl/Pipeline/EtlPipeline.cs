using System.Text.Json;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Etl.Core.Context;
using CorpInsightsTW.Etl.Pipeline.Extract;
using CorpInsightsTW.Etl.Pipeline.Load;
using CorpInsightsTW.Etl.Pipeline.Transform;

namespace CorpInsightsTW.Etl.Pipeline;

public class EtlPipeline(
    ILogger<EtlPipeline> logger,
    RuntimeConfig config,
    [FromKeyedServices("json")] IDataExtractor jsonExtractor,
    [FromKeyedServices("html")] IDataExtractor htmlExtractor,
    IDataTransformer transformer,
    IDataLoader loader)
{
    private readonly ILogger<EtlPipeline> _logger = logger;
    private readonly RuntimeConfig _config = config;

    private readonly IDataExtractor _jsonExtractor = jsonExtractor;
    private readonly IDataExtractor _htmlExtractor = htmlExtractor;
    private readonly IDataTransformer _transformer = transformer;
    private readonly IDataLoader _loader = loader;

    private static string GetIndent(int level) => new(' ', level * 4);

    public async Task RunAsync(CancellationToken ct = default, int indentLevel = 0)
    {
        StockMarket targetMarket = _config.Market;

        ct.ThrowIfCancellationRequested();

        var marketToFetch = targetMarket == StockMarket.All
            ? Enum.GetValues<StockMarket>().Where(m => m != StockMarket.All)
            : [targetMarket];

        var reportMarket = marketToFetch.ToList();

        foreach (var market in reportMarket)
        {
            await ExecPipelineAsync(market, ct, indentLevel);
        }
    }

    private async Task ExecPipelineAsync(StockMarket market, CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        StatementType targetType     = _config.Type;
        ListingStatus targetStatus   = _config.Status;
        XbrlTaxonomy  targetTaxonomy = _config.Taxonomy;
        DateOnly      targetDate     = _config.Date;

        ct.ThrowIfCancellationRequested();

        var statusList = targetStatus.ExpandForMarket(market).ToList();

        var taxonomyList = targetTaxonomy == XbrlTaxonomy.All
            ? Enum.GetValues<XbrlTaxonomy>().Where(t => t != XbrlTaxonomy.All).ToList()
            : [targetTaxonomy];

        var reportList = targetType == StatementType.All
            ? Enum.GetValues<StatementType>().Where(r => r != StatementType.All).ToList()
            : [targetType];

        var targetContexts = EtlContextBuilder
            .BuildContexts(market, reportList, statusList, taxonomyList, targetDate).ToList();

        _logger.LogInformation("{Indent}🏁 [Pipeline] ({Market}) 開始執行批次排程...", indent, market.ToCode());

        foreach (var context in targetContexts)
        {
            ct.ThrowIfCancellationRequested();
            
            await ExecPipelineStepAsync(context, ct, indentLevel + 1);
        }

        _logger.LogInformation("{Indent}✅ [Pipeline] ({Market}) 批次排程結束", indent, market.ToCode());
    }

    /// <summary>
    /// 單一規格組的 ETL 處理
    /// </summary>
    private async Task ExecPipelineStepAsync (EtlContext context, CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        string subIndent = GetIndent(indentLevel + 1); // 子項目專用縮排

        string tag = $"{context.Type.ToCode()}_{context.Status.ToCode()}_{context.Taxonomy.ToCode()}";
        
        string title = context.Type switch
        {
            StatementType.T187AP06 or 
            StatementType.T187AP07 => $"{context.Status.ToDisplay()} {context.Type.ToDisplay()} - {context.Taxonomy.ToDisplay()}",
            StatementType.T163SB20 => $"{context.Status.ToDisplay()} {context.Type.ToDisplay()}",
            _ => throw new NotSupportedException($"不支援的報表代號: {context.Type}"),
        };

        string message = $"[{context.Date:yyyyMMdd}] {tag} ({title})";

        _logger.LogInformation("{Indent}🏁 [Pipeline] ({Market}) 目標: {Message}", indent, context.Market.ToCode(), message);

        int currentBatchIndex = 0; 
        int fileTotalCount = 0;
        
        try
        {
            // 📥 1. Extract
            _logger.LogDebug("{Indent}📥 [Pipeline] 開始擷取 (Extract)...", indent);
            using var rawDoc = context.Type switch
            {
                StatementType.T187AP06 or 
                StatementType.T187AP07 => await _jsonExtractor.ExtractAsync(context, ct, indentLevel + 1),
                StatementType.T163SB20 => await _htmlExtractor.ExtractAsync(context, ct, indentLevel + 1),
                _ => throw new NotSupportedException($"不支援的報表代號: {context.Type}"),
            };

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
                await _loader.LoadAsync(context, batch, totalCount, ct, indentLevel + 1);
            }

            _logger.LogInformation("{Indent}✅ [Pipeline] 完畢，共處理 {Total} 筆。", indent, fileTotalCount);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogCritical(jsonEx, "{Indent}💥 [Pipeline] {Tag} JSON 解析嚴重失敗！",
                indent, tag);
            _logger.LogCritical(jsonEx, "{SubIndent}👉 錯誤原因: {ExMessage}",
                subIndent, jsonEx.Message);
            _logger.LogCritical(jsonEx, "{SubIndent}👉 JSON 錯誤位置: 行號 {LineNumber} | 該行字元位置 {BytePositionInLine}",
                subIndent, jsonEx.LineNumber, jsonEx.BytePositionInLine);
            _logger.LogCritical(jsonEx, "{SubIndent}👉 偵錯提示: 請檢查 DTO 定義的 [JsonPropertyName] 是否與政府最新欄位名稱一致，或數值型別是否不符。",
                subIndent);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Indent}❌ [Pipeline] {Tag} 處理時發生未預期異常！\n",
                indent, tag);
            _logger.LogError(ex, "{SubIndent}👉 執行上下文: 報表={Type}, 市場狀態={Status}, 分類={Taxonomy}, 日期={Date}\n",
                subIndent, context.Type, context.Status, context.Taxonomy, context.Date);
            _logger.LogError(ex, "{SubIndent}👉 當前進度: 已成功處理到第 {BatchIdx} 批次 (總共約 {Total} 筆)\n",
                subIndent, currentBatchIndex, fileTotalCount);
            _logger.LogError(ex, "{SubIndent}👉 異常類型: {ExType} | 訊息: {ExMessage}", 
                subIndent, ex.GetType().Name, ex.Message);
            throw;
        }
    }
}