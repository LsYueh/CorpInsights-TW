using System.Text.Json;
namespace CorpInsightsTW.Etl.Load;

public class ConsoleDataLoader(
    ILogger<ConsoleDataLoader> logger) : IDataLoader
{
    private readonly ILogger<ConsoleDataLoader> _logger = logger;

    public Task LoadAsync(IEnumerable<JsonElement> rows, CancellationToken cancellationToken)
    {
        int count = 0;

        foreach (JsonElement row in rows)
        {
            // 配合 CancellationToken，若外部取消則立刻停止輸出
            cancellationToken.ThrowIfCancellationRequested();

            // 🎯 直通列印：直接把單一列的完整 JSON 結構還原成字串輸出
            _logger.LogInformation("💾 [ConsoleLoad] 項目 [{Index}]: {RawText}", ++count, row.GetRawText());
        }

        _logger.LogInformation("✅ [ConsoleLoad] 資料流印出完畢，共處理 {Total} 筆。", count);

        return Task.CompletedTask;
    }
}