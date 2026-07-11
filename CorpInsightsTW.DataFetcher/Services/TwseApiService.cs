using System.Text.Json;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.DataFetcher.Services;

public class TwseApiService(
    ILogger<TwseApiService> logger,
    IHttpClientFactory httpClientFactory)
{
    private readonly ILogger<TwseApiService> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    private const string TwseApiTemplate = "https://openapi.twse.com.tw/v1/opendata/{0}_{1}_{2}";

    public async Task FetchFinancialDataAsync(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;

        string targetUrl = string.Format(TwseApiTemplate, apCode.ToCode(), status.ToCode(), taxonomy.ToCode());
        
        _logger.LogInformation("🌐 抓取資料: {Status} {Taxonomy} - {Name} ({Code})",
            status.ToDisplay(), taxonomy.ToDisplay(), apCode.ToDisplay(), apCode.ToCode());
        _logger.LogDebug("🔗 URL: {Url}", targetUrl);

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
                _logger.LogInformation("✅ 成功下載: {Status} {Taxonomy} - {Name} [{Count}] 筆",
                    status.ToDisplay(), taxonomy.ToDisplay(), apCode.ToDisplay(), dataCount);

                // TODO: ...
                
                if (dataCount > 0)
                {
                    var firstElement = jsonDocument.RootElement[0].GetRawText();
                    _logger.LogDebug("🔍 [{status}_{taxonomy} 資料抽樣]:\n{Json}", status.ToCode(), taxonomy.ToCode(), firstElement);
                }
            }
            else
            {
                _logger.LogWarning("⚠️ 證交所回傳格式異常，預期為 Array ，但實際拿到: {Kind}", jsonDocument.RootElement.ValueKind);
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

        // 爬蟲禮儀延遲
        await Task.Delay(500, stoppingToken);
    }
}