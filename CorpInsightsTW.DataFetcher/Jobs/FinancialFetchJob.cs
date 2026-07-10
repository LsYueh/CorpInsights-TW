using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CorpInsightsTW.DataFetcher.Jobs;

public class FinancialFetchJob(ILogger<FinancialFetchJob> logger)
{
    private readonly ILogger<FinancialFetchJob> _logger = logger;

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚀 開始執行財報下載與大寬表盲刷作業流程...");

        // 📝 這裡就是未來 6+6 大寬表資料落地與路由的黃金交叉點：
        // 步驟 1. 呼叫 Infrastructure 的 OpenApiClient 抓取最新的原始 JSON
        // 步驟 2. 將原始 JSON 丟給 Infrastructure 的 DataDispatcher (分流器)
        // 步驟 3. 分流器內部會自動做兩件事：
        //         A. 即時 Upsert 更新 company_industry_map 路由表
        //         B. 依行業別將數值丟進對應的 Mappers，最後 Upsert 進 t187ap07_ci 等大寬表

        _logger.LogInformation("🎉 本次排程財報盲刷作業已全數處理完畢！");
        await Task.CompletedTask;
    }
}