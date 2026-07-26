using CommandLine;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using CorpInsightsTW.Core.Logging;
using CorpInsightsTW.Core.Storage;
using CorpInsightsTW.Etl.Pipeline;
using CorpInsightsTW.Etl.Pipeline.Extract;
using CorpInsightsTW.Etl.Pipeline.Load;
using CorpInsightsTW.Etl.Pipeline.Transform;
using Microsoft.Extensions.Logging.Console;

namespace CorpInsightsTW.Etl;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var config = TryParseConfig(args);
        if (config == null) return 1;

        using var host = CreateHost(args, config);

        using var cts = new CancellationTokenSource();

        // 監聽控 Ctrl+C 事件
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("\n👋 偵測到使用者中斷指令 (Ctrl+C)，正在安全釋放網路連線...");
            cts.Cancel();
            eventArgs.Cancel = true; // 阻止作業系統立刻強行殺掉程式，給我們時間優雅退場
        };

        int exitCode;

        try
        {
            var pipeline = host.Services.GetRequiredService<EtlPipeline>();
            await pipeline.RunAsync(cts.Token);

            exitCode = 0;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("🛑 同步作業已被使用者安全取消。");
            exitCode = 130; // Linux Ctrl+C 標準結束代碼
        }
        catch (Exception)
        {
            exitCode = 1;
        }
        finally
        {
            await host.StopAsync();
        }

        return exitCode;
    }

    private static RuntimeConfig? TryParseConfig(string[] args)
    {
        var parseResult = Parser.Default.ParseArguments<CliOptions>(args);
        if (parseResult.Tag == ParserResultType.NotParsed)
        {
            return null;
        }

        var options = ((Parsed<CliOptions>)parseResult).Value;

        // 解析市場
        if (!Enum.TryParse<StockMarket>(options.Market, ignoreCase: true, out var market))
        {
            Console.WriteLine($"❌ 不合法的市場參數: '{options.Market}'");
            return null;
        }

        // 解析上市/櫃狀態
        if (!Enum.TryParse<ListingStatus>(options.Status, ignoreCase: true, out var status))
        {
            Console.WriteLine($"❌ 不合法的上市狀態參數: '{options.Status}'");
            return null;
        }

        // 解析申報分類
        if (!Enum.TryParse<XbrlTaxonomy>(options.Taxonomy, ignoreCase: true, out var taxonomy))
        {
            Console.WriteLine($"❌ 不合法的申報分類法參數: '{options.Taxonomy}'");
            return null;
        }

        // 解析報表代號
        if (!Enum.TryParse<T187ApCode>(options.ApCode, ignoreCase: true, out var apCode))
        {
            Console.WriteLine($"❌ 不合法的報表代號參數: '{options.ApCode}'");
            return null;
        }

        // 解析日期
        DateOnly date;
        if (string.IsNullOrWhiteSpace(options.Date))
        {
            date = DateOnly.FromDateTime(DateTime.Today);
        }
        else if (!DateOnly.TryParseExact(options.Date, "yyyyMMdd", out date))
        {
            Console.WriteLine($"❌ 不合法的日期參數: '{options.Date}' (格式應為 yyyyMMdd)");
            return null;
        }

        if (options.SkipDateCheck)
        {
            Console.WriteLine($"⚠️ 已啟用 --skip-date-check ，將強制跳過交易日校正！(目前指定的日期: {date:yyyy-MM-dd})");
        }
        else
        {
            // 交易日校正
            DateOnly adjustedDate = date.ToLastWeekday();
            if (adjustedDate != date)
            {
                Console.WriteLine(
                    $"ℹ️ 非交易日/假日操作，原始日期: {date:yyyy-MM-dd} ({date.DayOfWeek}), " +
                    $"市場: {market} -> 自動調整目錄日期為最近交易日: {adjustedDate:yyyy-MM-dd} ({adjustedDate.DayOfWeek})"
                );

                date = adjustedDate;
            }
        }

        // 建立 Config
        try
        {
            // 用 try-catch 攔截 RuntimeConfig 建構子丟出的市場/狀態不匹配例外
            return new RuntimeConfig(market, status, taxonomy, apCode, date, options.DryRun);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ 參數組合錯誤: {ex.Message}");
            return null;
        }
    }

    private static IHost CreateHost(string[] args, RuntimeConfig runtimeConfig)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options =>
        {
            options.FormatterName = CleanConsoleFormatter.FormatterName; // 指定使用 CleanConsole
        });
        builder.Logging.AddConsoleFormatter<CleanConsoleFormatter, ConsoleFormatterOptions>();

        builder.Services.AddSingleton(runtimeConfig);

        // Storage
        string? customStoragePath = builder.Configuration["Storage:RawDataPath"];
        builder.Services.AddSingleton(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<LocalRawDataStorage>>();
            return new LocalRawDataStorage(logger, customStoragePath);
        });

        builder.Services.AddTransient<IDataExtractor  , JsonFileDataExtractor>();
        builder.Services.AddTransient<IDataTransformer, JsonDataTransformer>();
        builder.Services.AddTransient<IDataLoader>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<T187DataLoader>>();
            var config = sp.GetRequiredService<IConfiguration>();
            
            string connectionString = config.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("找不到 DefaultConnection 連線字串設定");

            return new T187DataLoader(logger, runtimeConfig, connectionString);
        });
        builder.Services.AddTransient<EtlPipeline>();

        return builder.Build();
    }
}