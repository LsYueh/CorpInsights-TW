using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Core.Storage;
using CorpInsightsTW.DataFetcher.Core.Common;

namespace CorpInsightsTW.DataFetcher.Services;

public class TwseApiService(
    ILogger<TwseApiService> logger,
    IHttpClientFactory httpClientFactory,
    RuntimeConfig config,
    LocalRawDataStorage storage)
{
    private readonly ILogger<TwseApiService> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly RuntimeConfig _config = config;
    private readonly LocalRawDataStorage _storage = storage;

    private static string GetIndent(int level) => new(' ', level * 4);

    public async Task FetchFinancialDataAsync(FetchContext context, CancellationToken stoppingToken, int indentLevel = 0)
    {
        if (stoppingToken.IsCancellationRequested) return;

        string indent = GetIndent(indentLevel);

        string tag = $"{context.ApCode.ToCode()}_{context.Status.ToCode()}_{context.Taxonomy.ToCode()}";
        string title = $"{context.Status.ToDisplay()} {context.ApCode.ToDisplay()} - {context.Taxonomy.ToDisplay()}";

        var storageContext = new StorageContext(context.ApCode, context.Status, context.Taxonomy);

        // 檔案檢查
        if (_storage.Exists(storageContext, indentLevel + 1))
        {
            _logger.LogInformation("{Indent}🚀 檔案: {Tag} ({Title}) 已存在，跳過 HTTP 請求。", indent, tag, title);
            return;
        }

        string targetUrl = GetTargetUrl(context.ApCode, context.Status, context.Taxonomy, indentLevel);

        using var client = _httpClientFactory.CreateClient();

        try
        {
            // 發送網路請求
            using var response = await client.GetAsync(targetUrl, HttpCompletionOption.ResponseHeadersRead, stoppingToken);
            
            if (IsValidHttpResponse(response, indentLevel + 1))
            {
                // 取得網路串流並 Pipe 到本地檔案流
                using var responseStream = await response.Content.ReadAsStreamAsync(stoppingToken);

                // Pipe 到本地滾動檔案流
                {
                    // Note: 區域範疇區隔，確保寫入完畢並 Flush 後，立刻關檔釋放鎖定
                    
                    using var fileStream = _storage.CreateWritableStream(storageContext, indentLevel + 1);
                    await responseStream.CopyToAsync(fileStream, stoppingToken);
                    await fileStream.FlushAsync(stoppingToken);
                }

                _logger.LogInformation("{Indent}✅ 資料成功寫入", indent);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "{Indent}❌ 呼叫證交所 OpenAPI 時發生網路連線或 HTTP 狀態碼錯誤 [URL: {Url}]", indent, targetUrl);

            _storage.Delete(storageContext, indentLevel + 1);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Indent}❌ 檔案下載或落地儲存時發生非預期錯誤 [URL: {Url}]", indent, targetUrl);

            _storage.Delete(storageContext, indentLevel + 1);

            throw;
        }
        
        // 爬蟲禮儀延遲
        await Task.Delay(1000, stoppingToken);
    }

    public string GetTargetUrl(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        string baseUrl = _config.TwseRootUrl.TrimEnd('/'); // 確保 RootUrl 結尾乾淨
        string targetUrl = $"{baseUrl}/opendata/{apCode.ToCode()}_{status.ToCode()}_{taxonomy.ToCode()}";
        
        _logger.LogInformation("{Indent}🌐 抓取資料: {Status} {Taxonomy} - {Name} ({Code})", indent,
            status.ToDisplay(), taxonomy.ToDisplay(), apCode.ToDisplay(), apCode.ToCode());
        _logger.LogDebug("{Indent}🔗 URL: {Url}", indent, targetUrl);

        return targetUrl;
    }

    public bool IsValidHttpResponse(HttpResponseMessage response, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        var targetUrl = response.RequestMessage?.RequestUri;

        // 檢查 HTTP Status Code 是否正常
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("{Indent}⚠️ HTTP 請求未成功 [Status: {Status}, URL: {Url}]", 
                indent, response.StatusCode, targetUrl);

            return false;
        }

        // 檢查 Header 的 Content-Type 是否為 application/json
        var mediaType = response.Content.Headers.ContentType?.MediaType;
        if (mediaType == null || !mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("{Indent}⚠️ 收到非預期的內容型態: {MediaType}，非 JSON 格式 [URL: {Url}]", 
                indent, mediaType ?? "null", targetUrl);

            return false;
        }

        return true;
    }
}