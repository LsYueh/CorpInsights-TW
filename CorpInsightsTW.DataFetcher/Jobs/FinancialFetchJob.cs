using System.Text.Json;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FinancialFetchJob(
    ILogger<FinancialFetchJob> logger,
    IHttpClientFactory httpClientFactory,
    FetchRunConfig config)
{
    private readonly ILogger<FinancialFetchJob> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly FetchRunConfig _config = config;

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Taxonomy      targetTaxonomy = _config.TargetTaxonomy;
        ListingStatus targetStatus   = _config.Status;

        _logger.LogInformation("🎬 FinancialFetchJob 開始發動。目標過濾狀態: [ {Status} ], 分類: [ {Taxonomy} ]", targetStatus, targetTaxonomy);

        stoppingToken.ThrowIfCancellationRequested();

        try
        {
            // 1. 決定目標市場狀態清單 (如果是 All，就抓出排除 All 以外的所有合法實體列舉)
            var marketsToFetch = targetStatus == ListingStatus.All
                ? Enum.GetValues<ListingStatus>().Where(m => m != ListingStatus.All)
                : [targetStatus];

            // 2. 決定目標產業分類清單 (如果是 All，就抓出排除 All 以外的所有合法實體列舉)
            var taxonomiesToFetch = targetTaxonomy == Taxonomy.All
                ? Enum.GetValues<Taxonomy>().Where(t => t != Taxonomy.All)
                : [targetTaxonomy];

            _logger.LogInformation("📊 [分流調度解構完成] 預計執行組合數: {Count} 組", marketsToFetch.Count() * taxonomiesToFetch.Count());

            // 3. 發動同步
            foreach (var taxonomy in taxonomiesToFetch)
            {
                foreach (var market in marketsToFetch)
                {
                    stoppingToken.ThrowIfCancellationRequested();
                    
                    _logger.LogInformation("⚡ [佇列執行] 正在派發作業 -> 狀態: {Market}, 分類: {Taxonomy} ...", market, taxonomy);
                    await FetchBalanceSheetsAsync(market, taxonomy, stoppingToken);
                }
            }

            _logger.LogInformation("✨ FinancialFetchJob 本次批次同步調度安全結束。");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ FinancialFetchJob 在調度執行期間發生未預期崩潰");
            throw; 
        }
    }

    /// <summary>
    /// 資產負債表 (T187AP07)
    /// </summary>
    /// <param name="status">上市狀態</param>
    /// <param name="taxonomy">財報分類</param>
    private async Task FetchBalanceSheetsAsync(ListingStatus status, Taxonomy taxonomy, CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;

        const string TwseApiTemplate = "https://openapi.twse.com.tw/v1/opendata/t187ap07_{0}_{1}";

        // 動態組裝目標 URL
        string targetUrl = string.Format(TwseApiTemplate, status.ToCode(), taxonomy.ToCode());
        
        _logger.LogInformation("🌐 正在抓取證交所財報資料: 資產負債表 (T187AP07) [狀態: {Market}, 分類: {Industry}]", status, taxonomy);
        _logger.LogDebug("🔗 請求網址: {Url}", targetUrl);

        using var client = _httpClientFactory.CreateClient();
        
        try
        {
            using var response = await client.GetAsync(targetUrl, HttpCompletionOption.ResponseHeadersRead, stoppingToken);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync(stoppingToken);
            using var jsonDocument = await JsonDocument.ParseAsync(responseStream, cancellationToken: stoppingToken);
            
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                int dataCount = jsonDocument.RootElement.GetArrayLength();
                _logger.LogInformation("✅ 成功下載！[狀態: {Market}, 分類: {Industry}] 總計抓取到: [ {Count} ] 筆資料。", status, taxonomy, dataCount);

                // 偵錯小確幸：抽樣印出第一筆 JSON 快照
                if (dataCount > 0)
                {
                    var firstElement = jsonDocument.RootElement[0].GetRawText();
                    _logger.LogDebug("🔍 [{Market}_{Industry} 原始資料抽樣]:\n{Json}", status, taxonomy, firstElement);
                }
            }
            else
            {
                _logger.LogWarning("⚠️ 證交所回傳格式異常，預期為 Array，但實際拿到: {Kind}", jsonDocument.RootElement.ValueKind);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "❌ 呼叫證交所 OpenAPI 時發生網路連線或 HTTP 狀態碼錯誤 [URL: {Url}]", targetUrl);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "❌ 解析證交所 JSON 數據時發生結構錯誤 [URL: {Url}]", targetUrl);
            throw;
        }

        await Task.Delay(500, stoppingToken);
    }
}