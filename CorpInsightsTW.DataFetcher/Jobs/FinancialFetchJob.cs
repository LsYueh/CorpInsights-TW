using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.DataFetcher.Core.Common;
using CorpInsightsTW.DataFetcher.Services;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FinancialFetchJob(
    ILogger<FinancialFetchJob> logger,
    TwseApiService twseApiService,
    RuntimeConfig config)
{
    private readonly ILogger<FinancialFetchJob> _logger = logger;
    private readonly TwseApiService _twseApiService = twseApiService;
    private readonly RuntimeConfig _config = config;

    private static string GetIndent(int level) => new(' ', level * 4);

    public async Task ExecuteAsync(CancellationToken stoppingToken, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        StockMarket   targetMarket   = _config.Market;
        XbrlTaxonomy  targetTaxonomy = _config.Taxonomy;
        ListingStatus targetStatus   = _config.Status;
        T187ApCode    targetApCode   = _config.ApCode;

        _logger.LogInformation("{Indent}🎬 發動 HTTP 請求: [{Market}] {Status} {Taxonomy} - {Name}", indent, 
            targetMarket.ToDisplay(), targetStatus.ToDisplay(), targetTaxonomy.ToDisplay(), targetApCode.ToDisplay());

        stoppingToken.ThrowIfCancellationRequested();

        try
        {
            // Note: 清單過濾方式 (如果是 All，就抓出排除 All 以外的所有合法實體列舉)
            
            // var marketToFetch = targetMarket == StockMarket.All
            //     ? Enum.GetValues<StockMarket>().Where(m => m != StockMarket.All)
            //     : [targetMarket];

            var statusToFetch = targetStatus == ListingStatus.All
                ? Enum.GetValues<ListingStatus>().Where(m => m != ListingStatus.All)
                : [targetStatus];

            var taxonomiesToFetch = targetTaxonomy == XbrlTaxonomy.All
                ? Enum.GetValues<XbrlTaxonomy>().Where(t => t != XbrlTaxonomy.All)
                : [targetTaxonomy];

            var reportsToFetch = targetApCode == T187ApCode.All
                ? Enum.GetValues<T187ApCode>().Where(r => r != T187ApCode.All)
                : [targetApCode];

            _logger.LogInformation("{Indent}📊 預計執行組合數: {Count} 組", indent, 
                statusToFetch.Count() * taxonomiesToFetch.Count() * reportsToFetch.Count());

            // foreach (var market in marketToFetch)
            // {
                foreach (var taxonomy in taxonomiesToFetch)
                {
                    foreach (var status in statusToFetch)
                    {
                        // await FetchReportsGroupAsync(market, taxonomy, status, reportsToFetch, stoppingToken, indentLevel + 1);
                        await FetchReportsGroupAsync(StockMarket.TWSE, taxonomy, status, reportsToFetch, stoppingToken, indentLevel + 1);
                    }
                }
            // }

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
        CancellationToken stoppingToken, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        _logger.LogInformation("{Indent}⚡ HTTP 請求: [{Market}] {Status} - {Taxonomy}", indent, market.ToCode(), status.ToDisplay(), taxonomy.ToDisplay());

        foreach (var apCode in apCodes)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var context = new FetchContext(market, apCode, status, taxonomy);

            await _twseApiService.FetchFinancialDataAsync(context, stoppingToken, indentLevel + 1);
        }
    }
}