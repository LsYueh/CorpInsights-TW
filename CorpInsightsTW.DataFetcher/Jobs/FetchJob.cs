using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.DataFetcher.Core.Common;
using CorpInsightsTW.DataFetcher.Services;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FetchJob(
    ILogger<FetchJob> logger,
    TwseApiService twseApiService,
    RuntimeConfig config)
{
    private readonly ILogger<FetchJob> _logger = logger;
    private readonly TwseApiService _twseApiService = twseApiService;
    private readonly RuntimeConfig _config = config;

    private static string GetIndent(int level) => new(' ', level * 4);

    public async Task ExecAsync(CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        StockMarket targetMarket = _config.Market;

        ct.ThrowIfCancellationRequested();

        try
        {
            // Note: 清單過濾方式 (如果是 All，就抓出排除 All 以外的所有合法實體列舉)
            
            var marketToFetch = targetMarket == StockMarket.All
                ? Enum.GetValues<StockMarket>().Where(m => m != StockMarket.All)
                : [targetMarket];

            foreach (var market in marketToFetch)
            {
                await FetchReportsAsync(market, ct, indentLevel);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Indent}❌ 執行期間發生未預期錯誤", indent);
            throw; 
        }
    }

    private async Task FetchReportsAsync(
        StockMarket market, CancellationToken ct, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        XbrlTaxonomy  targetTaxonomy = _config.Taxonomy;
        ListingStatus targetStatus   = _config.Status;
        T187ApCode    targetApCode   = _config.ApCode;

        ct.ThrowIfCancellationRequested();

        _logger.LogInformation("{Indent}🎬 發動 HTTP 請求: [{Market}] {Status} {Taxonomy} - {Name}", indent, 
            market.ToDisplay(), targetStatus.ToDisplay(), targetTaxonomy.ToDisplay(), targetApCode.ToDisplay());

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

        foreach (var taxonomy in taxonomiesToFetch)
        {
            foreach (var status in statusToFetch)
            {
                await FetchReportsGroupAsync(market, taxonomy, status, reportsToFetch, ct, indentLevel + 1);
            }
        }

        _logger.LogInformation("{Indent}✨ 批次 HTTP 請求安全結束。", indent);
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

            Task fetchTask = market switch
            {
                StockMarket.TWSE => _twseApiService.FetchDataAsync(context, ct, indentLevel + 1),
                StockMarket.TPEX => throw new NotImplementedException(),
                _ => throw new NotSupportedException($"未知的 T187Ap06 分類: {context.Taxonomy.ToCode()}")
            };

            await fetchTask;
        }
    }
}