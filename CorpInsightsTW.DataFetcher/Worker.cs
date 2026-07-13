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
        try
        {
            _logger.LogInformation("⚡ 開始執行同步作業...");
            
            await ExecuteJobAsync(stoppingToken);
            
            _logger.LogInformation("📴 同步作業已順利完成");
        }
        catch (OperationCanceledException)
        {
            // 當外部（Linux systemd 或 Windows Service）下達停止指令時，會丟出此異常，屬於正常優雅退場
            _logger.LogWarning("👋 收到作業系統中止訊號，正在確保當前連線完全安全釋放...");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "❌ DataFetcher 發生未預期的錯誤");

            Environment.ExitCode = 1;
        }
        finally
        {
            _lifetime.StopApplication();
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