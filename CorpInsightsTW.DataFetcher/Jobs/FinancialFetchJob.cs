using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.DataFetcher.Services;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FinancialFetchJob(
    ILogger<FinancialFetchJob> logger,
    TwseApiService twseApiService,
    FetchRunConfig config)
{
    private readonly ILogger<FinancialFetchJob> _logger = logger;
    private readonly TwseApiService _twseApiService = twseApiService;
    private readonly FetchRunConfig _config = config;

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        XbrlTaxonomy  targetTaxonomy = _config.Taxonomy;
        ListingStatus targetStatus   = _config.Status;
        T187ApCode    targetApCode   = _config.ApCode;

        _logger.LogInformation("🎬 發動: {Status} {Taxonomy} - {Name}", 
            targetStatus.ToDisplay(), targetTaxonomy.ToDisplay(), targetApCode.ToDisplay());

        stoppingToken.ThrowIfCancellationRequested();

        try
        {
            // Note: 清單過濾方式 (如果是 All，就抓出排除 All 以外的所有合法實體列舉)

            var statusToFetch = targetStatus == ListingStatus.All
                ? Enum.GetValues<ListingStatus>().Where(m => m != ListingStatus.All)
                : [targetStatus];

            var taxonomiesToFetch = targetTaxonomy == XbrlTaxonomy.All
                ? Enum.GetValues<XbrlTaxonomy>().Where(t => t != XbrlTaxonomy.All)
                : [targetTaxonomy];

            var reportsToFetch = targetApCode == T187ApCode.All
                ? Enum.GetValues<T187ApCode>().Where(r => r != T187ApCode.All)
                : [targetApCode];

            _logger.LogInformation("📊 預計執行組合數: {Count} 組", 
                statusToFetch.Count() * taxonomiesToFetch.Count() * reportsToFetch.Count());

            foreach (var taxonomy in taxonomiesToFetch)
            {
                foreach (var status in statusToFetch)
                {
                    _logger.LogInformation("⚡ 派發作業: {Status} {Taxonomy}", status.ToDisplay(), taxonomy.ToDisplay());

                    foreach (var apCode in reportsToFetch)
                    {
                        stoppingToken.ThrowIfCancellationRequested();

                        await _twseApiService.FetchFinancialDataAsync(apCode, status, taxonomy, stoppingToken);
                    }
                }
            }

            _logger.LogInformation("✨ FinancialFetchJob 本次批次同步調度安全結束。");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ FinancialFetchJob 在執行期間發生未預期錯誤");
            throw; 
        }
    }
}