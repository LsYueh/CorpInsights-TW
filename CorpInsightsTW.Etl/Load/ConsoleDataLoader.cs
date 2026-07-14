using System.Text.Json;
namespace CorpInsightsTW.Etl.Load;

public class ConsoleDataLoader(ILogger<ConsoleDataLoader> logger) : IDataLoader
{
    private readonly ILogger<ConsoleDataLoader> _logger = logger;

    private readonly JsonSerializerOptions options = new() { WriteIndented = true };

    public Task LoadAsync(JsonDocument data, CancellationToken cancellationToken)
    {
        _logger.LogInformation("💾 執行資料載入 (輸出至控制台)...");

        // 格式化印出，確認剛才 Extract 出來的 JSON 內容完全吻合
        Console.WriteLine(JsonSerializer.Serialize(data, options));

        return Task.CompletedTask;
    }
}