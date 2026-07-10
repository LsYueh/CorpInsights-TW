using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FinancialFetchJob(
    ILogger<FinancialFetchJob> logger,
    FetchRunConfig config)
{
    private readonly ILogger<FinancialFetchJob> _logger = logger;
    private readonly FetchRunConfig _config = config;
    
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 🎯 成功攔截！從 Worker 一路穿越過來的作戰指令
        string targetIndustry = _config.Industry;

        _logger.LogInformation("🎬 FinancialFetchJob 開始發動。目標過濾產業為: [ {Industry} ]", targetIndustry);

        // 檢查系統中止訊號
        stoppingToken.ThrowIfCancellationRequested();

        try
        {
            // ========================================================
            // 🧭 核心分流調度邏輯
            // ========================================================
            switch (targetIndustry)
            {
                case "all":
                    _logger.LogInformation("📊 [分流調度] 準備執行全量盲刷：一般產業(ci) + 金控產業(fh)...");
                    await FetchGeneralIndustryAsync(stoppingToken);
                    await FetchFinancialIndustryAsync(stoppingToken);
                    break;

                case "ci":
                    _logger.LogInformation("🏭 [分流調度] 僅執行：一般產業(ci) 全量同步...");
                    await FetchGeneralIndustryAsync(stoppingToken);
                    break;

                case "fh":
                    _logger.LogInformation("🏦 [分流調度] 僅執行：金控產業(fh) 全量同步...");
                    await FetchFinancialIndustryAsync(stoppingToken);
                    break;

                default:
                    _logger.LogWarning("⚠️ 偵測到不合法的產業參數: '{Industry}'，本次作業放棄執行。", targetIndustry);
                    break;
            }

            _logger.LogInformation("✨ FinancialFetchJob 本次批次同步調度安全結束。");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ FinancialFetchJob 在調度執行期間發生未預期崩潰");
            throw; // 向上拋出，讓 Worker 的 try-catch 能夠捕捉並記錄日誌
        }
    }

    /// <summary>
    /// 🏭 抓取一般產業財報 (t187ap07_ci)
    /// </summary>
    private async Task FetchGeneralIndustryAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;
        _logger.LogInformation("⏳ 正在呼叫證交所 OpenAPI 下載 [一般產業] 原始大寬表 JSON Array...");
        
        // TODO: 未來在此調度 OpenApiClient 抓取並洗入 DB
        await Task.Delay(500, stoppingToken); // 模擬 I/O 延遲
    }

    /// <summary>
    /// 🏦 抓取金控產業財報 (t187ap07_fh)
    /// </summary>
    private async Task FetchFinancialIndustryAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;
        _logger.LogInformation("⏳ 正在呼叫證交所 OpenAPI 下載 [金控產業] 原始大寬表 JSON Array...");
        
        // TODO: 未來在此調度 OpenApiClient 抓取並洗入 DB
        await Task.Delay(500, stoppingToken); // 模擬 I/O 延遲
    }
}