using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.DataFetcher.Core.Common;
using CorpInsightsTW.DataFetcher.Services;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FetchJob(
    ILogger<FetchJob> logger,
    OpenApiService openApiService,
    RuntimeConfig config)
{
    private readonly ILogger<FetchJob> _logger = logger;
    private readonly OpenApiService _openApiService = openApiService;
    private readonly RuntimeConfig _config = config;

    private static string GetIndent(int level) => new(' ', level * 4);

    public async Task ExecAsync(CancellationToken ct, int indentLevel = 0)
    {
        StockMarket targetMarket = _config.Market;

        ct.ThrowIfCancellationRequested();
            
        var marketToFetch = targetMarket == StockMarket.All
            ? Enum.GetValues<StockMarket>().Where(m => m != StockMarket.All)
            : [targetMarket];

        var reportMarket = marketToFetch.ToList();

        foreach (var market in reportMarket)
        {
            await FetchReportsAsync(market, ct, indentLevel);
        }
    }

    private async Task FetchReportsAsync(StockMarket market, CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        XbrlTaxonomy  targetTaxonomy = _config.Taxonomy;
        ListingStatus targetStatus   = _config.Status;
        T187ApCode    targetApCode   = _config.ApCode;

        ct.ThrowIfCancellationRequested();

         _logger.LogInformation("{Indent}🎬 發動 HTTP 請求: [{Market}] {Status} {Taxonomy} - {Name}", indent, 
            market.ToDisplay(), targetStatus.ToDisplay(), targetTaxonomy.ToDisplay(), targetApCode.ToDisplay());

        var statusToFetch = targetStatus.ExpandForMarket(market);

        var taxonomiesToFetch = targetTaxonomy == XbrlTaxonomy.All
            ? Enum.GetValues<XbrlTaxonomy>().Where(t => t != XbrlTaxonomy.All)
            : [targetTaxonomy];

        var reportsToFetch = targetApCode == T187ApCode.All
            ? Enum.GetValues<T187ApCode>().Where(r => r != T187ApCode.All)
            : [targetApCode];

        try
        {
            // 💡 轉成 List/Array 避免 LINQ 多次 Evaluate (Count() + foreach)
            var statusList   = statusToFetch.ToList();
            var taxonomyList = taxonomiesToFetch.ToList();
            var reportList   = reportsToFetch.ToList();
            
            _logger.LogInformation("{Indent}📊 預計執行組合數: {Count} 組", indent, 
                statusList.Count * taxonomyList.Count * reportList.Count);

            foreach (var taxonomy in taxonomyList)
            {
                foreach (var status in statusList)
                {
                    await FetchReportsGroupAsync(market, taxonomy, status, reportList, ct, indentLevel + 1);
                }
            }

            _logger.LogInformation("{Indent}✨ 批次 HTTP 請求安全結束。", indent);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Indent}❌ 執行期間發生未預期錯誤", indent);
            throw; 
        }
    }

    private async Task FetchReportsGroupAsync(
        StockMarket market,
        XbrlTaxonomy taxonomy, ListingStatus status, IEnumerable<T187ApCode> apCodes,
        CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        _logger.LogInformation("{Indent}⚡ HTTP 請求: [{Market}] {Status} - {Taxonomy}", indent, market.ToCode(), status.ToDisplay(), taxonomy.ToDisplay());

        foreach (var apCode in apCodes)
        {
            ct.ThrowIfCancellationRequested();

            var context = new FetchContext(market, apCode, status, taxonomy);
            await _openApiService.FetchDataAsync(context, ct, indentLevel + 1);
        }
    }
}