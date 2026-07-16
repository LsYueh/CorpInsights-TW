using System.Text.Json;
namespace CorpInsightsTW.Etl.Load;

public class ConsoleDataLoader(
    ILogger<ConsoleDataLoader> logger) : IDataLoader
{
    private readonly ILogger<ConsoleDataLoader> _logger = logger;

    // 追蹤跨批次累計的總筆數
    private int _totalProcessedCount = 0;

    public Task LoadAsync(IReadOnlyList<JsonElement> batch, int fileTotalCount, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        int currentTotal = Interlocked.Add(ref _totalProcessedCount, batch.Count);

        _logger.LogInformation("💾 [ConsoleLoad] 進度: {CurrentTotal}/{FileTotal}", 
            currentTotal, fileTotalCount);

        // 當處理到檔案的最後一筆時，重置計數器（方便下一個檔案進來時重新計算）
        if (currentTotal >= fileTotalCount)
            Interlocked.Exchange(ref _totalProcessedCount, 0);

        // TODO: ...

        return Task.CompletedTask;
    }
}