using CorpInsightsTW.DataFetcher.Jobs;

namespace CorpInsightsTW.DataFetcher;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider serviceProvider,
    IHostApplicationLifetime lifetime,
    FetchRunConfig config) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly FetchRunConfig _config = config;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚀 DataFetcher 服務核心初始化啟動 | 運行模式: {Mode}", _config.Mode);

        try
        {
            if (_config.Mode == "once")
            {
                // ========================================================
                // 🎯 嚴謹的 Once (單次刺客) 模式處理
                // ========================================================
                _logger.LogInformation("⚡ [Once 模式] 開始執行單次同步作業...");
                
                // 傳入完整的 stoppingToken，確保下載或寫入 MySQL 途中隨時可被作業系統安全中止
                await ExecuteJobAsync(stoppingToken);
                
                _logger.LogInformation("📴 [Once 模式] 同步作業已順利完成");
            }
            else
            {
                // ========================================================
                // 🎯 嚴謹的 Daemon (後台哨兵) 模式處理
                // ========================================================
                _logger.LogInformation("🕒 [Daemon 模式] 進入背景排程循環，每 12 小時自動執行...");
                
                // 建立精準的定時器，並綁定 stoppingToken
                using var timer = new PeriodicTimer(TimeSpan.FromHours(12));

                // 精準等待下一次的 Tick，且隨時監聽停止訊號
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation("⏰ [Daemon 模式] 達預定時間，發動排程抓取任務...");
                    await ExecuteJobAsync(stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // 當外部（Linux systemd 或 Windows Service）下達停止指令時，會丟出此異常，屬於正常優雅退場
            _logger.LogWarning("👋 收到作業系統中止訊號，正在確保當前連線完全安全釋放...");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "❌ DataFetcher 發生未預期的錯誤");
        }
        finally
        {
            // 🔒 不管是 once 跑完，還是 daemon 被關閉，通通走這裡進行「進程拔插頭」
            // 這能確保所有 using 區塊的 MySQL 連線、記憶體 Stream 都在執行緒結束後「完全釋放」，才正式關閉全系統
            if (_config.Mode == "once" || stoppingToken.IsCancellationRequested)
            {
                _lifetime.StopApplication();
            }
        }
    }

    private async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;

        using var scope = _serviceProvider.CreateScope();
        var job = scope.ServiceProvider.GetRequiredService<FinancialFetchJob>();
        
        try
        {
            await job.ExecuteAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ 執行 FinancialFetchJob 時發生未預期錯誤");
        }
    }
}