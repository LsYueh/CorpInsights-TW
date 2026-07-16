using System.Text.Json;
using CorpInsightsTW.Etl.Common;

namespace CorpInsightsTW.Etl.Load;

public class ConsoleDataLoader(
    ILogger<ConsoleDataLoader> logger) : IDataLoader
{
    private readonly ILogger<ConsoleDataLoader> _logger = logger;

    private static string GetIndent(int level) => new(' ', level * 4);

    // 追蹤跨批次累計的總筆數
    private int _totalProcessedCount = 0;

    public Task LoadAsync(EtlContext context,
        IReadOnlyList<JsonElement> batch, int fileTotalCount,
        CancellationToken cancellationToken, int indentLevel = 0)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string indent = GetIndent(indentLevel);

        int currentTotal = Interlocked.Add(ref _totalProcessedCount, batch.Count);

        _logger.LogInformation("{Indent}💾 進度: {CurrentTotal}/{FileTotal}",
            indent, currentTotal, fileTotalCount);

        // 當處理到檔案的最後一筆時，重置計數器（方便下一個檔案進來時重新計算）
        if (currentTotal >= fileTotalCount)
            Interlocked.Exchange(ref _totalProcessedCount, 0);

        // TODO: ...

        return Task.CompletedTask;
    }
}