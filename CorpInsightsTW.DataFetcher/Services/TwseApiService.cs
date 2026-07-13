using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Infrastructure.Storage;

namespace CorpInsightsTW.DataFetcher.Services;

public class TwseApiService(
    ILogger<TwseApiService> logger,
    IHttpClientFactory httpClientFactory,
    FetchRunConfig config,
    LocalRawDataStorage storage)
{
    private readonly ILogger<TwseApiService> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly FetchRunConfig _config = config;
    private readonly LocalRawDataStorage _storage = storage;

    public async Task FetchFinancialDataAsync(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;

        // 檔案檢查
        if (_storage.Exists(apCode, status, taxonomy))
        {
            _logger.LogInformation("🚀 [Cache Hit] 落地檔案已存在，跳過網路請求。");
            return;
        }

        string targetUrl = GetTargetUrl(apCode, status, taxonomy);

        using var client = _httpClientFactory.CreateClient();

        try
        {
            // 發送網路請求
            using var response = await client.GetAsync(targetUrl, HttpCompletionOption.ResponseHeadersRead, stoppingToken);
            response.EnsureSuccessStatusCode();

            // 取得網路串流並
            using var responseStream = await response.Content.ReadAsStreamAsync(stoppingToken);

            // Pipe 到本地滾動檔案流
            {
                // Note: 區域範疇區隔，確保寫入完畢並 Flush 後，立刻關檔釋放鎖定
                
                using var fileStream = _storage.CreateWritableStream(apCode, status, taxonomy);
                await responseStream.CopyToAsync(fileStream, stoppingToken);
                await fileStream.FlushAsync(stoppingToken);
            }

            _logger.LogInformation("✅ 資料成功寫入");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "❌ 呼叫證交所 OpenAPI 時發生網路連線或 HTTP 狀態碼錯誤 [URL: {Url}]", targetUrl);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ 檔案下載或落地儲存時發生非預期錯誤 [URL: {Url}]", targetUrl);
            throw;
        }

        // 爬蟲禮儀延遲
        await Task.Delay(500, stoppingToken);
    }

    public string GetTargetUrl(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy)
    {
        string baseUrl = _config.TwseRootUrl.TrimEnd('/'); // 確保 RootUrl 結尾乾淨
        string targetUrl = $"{baseUrl}/opendata/{apCode.ToCode()}_{status.ToCode()}_{taxonomy.ToCode()}";
        
        _logger.LogInformation("🌐 抓取資料: {Status} {Taxonomy} - {Name} ({Code})",
            status.ToDisplay(), taxonomy.ToDisplay(), apCode.ToDisplay(), apCode.ToCode());
        _logger.LogDebug("🔗 URL: {Url}", targetUrl);

        return targetUrl;
    }
}