using CommandLine;

using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Etl.Core.Extract;
using CorpInsightsTW.Etl.Core.Load;
using CorpInsightsTW.Etl.Core.Logging;
using CorpInsightsTW.Etl.Core.Pipeline;
using CorpInsightsTW.Etl.Core.Transform;
using CorpInsightsTW.Infrastructure.Storage;
using Microsoft.Extensions.Logging.Console;

namespace CorpInsightsTW.Etl;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var runConfig = TryParseConfig(args);
        if (runConfig == null) return 1;

        using var host = CreateHost(args, runConfig);

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

    private static EtlRunConfig? TryParseConfig(string[] args)
    {
        var parseResult = Parser.Default.ParseArguments<CliOptions>(args);
        if (parseResult.Tag == ParserResultType.NotParsed)
        {
            return null;
        }

        var options = ((Parsed<CliOptions>)parseResult).Value;

        if (!Enum.TryParse<ListingStatus>(options.Status, ignoreCase: true, out var status))
        {
            Console.WriteLine($"❌ 不合法的上市狀態參數: '{options.Status}'");
            return null;
        }

        if (!Enum.TryParse<XbrlTaxonomy>(options.Taxonomy, ignoreCase: true, out var taxonomy))
        {
            Console.WriteLine($"❌ 不合法的申報分類法參數: '{options.Taxonomy}'");
            return null;
        }

        if (!Enum.TryParse<T187ApCode>(options.ApCode, ignoreCase: true, out var apCode))
        {
            Console.WriteLine($"❌ 不合法的報表代號參數: '{options.ApCode}'");
            return null;
        }

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

        return new EtlRunConfig(status, taxonomy, apCode, date);
    }

    private static IHost CreateHost(string[] args, EtlRunConfig runConfig)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options =>
        {
            options.FormatterName = CleanConsoleFormatter.FormatterName; // 指定使用 CleanConsole
        });
        builder.Logging.AddConsoleFormatter<CleanConsoleFormatter, ConsoleFormatterOptions>();

        builder.Services.AddSingleton(runConfig);

        // Storage
        string? customStoragePath = builder.Configuration["Storage:RawDataPath"];
        builder.Services.AddSingleton(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<LocalRawDataStorage>>();
            return new LocalRawDataStorage(logger, customStoragePath);
        });

        builder.Services.AddTransient<IDataExtractor  , JsonFileDataExtractor>();
        builder.Services.AddTransient<IDataTransformer, JsonDataTransformer>();
        builder.Services.AddTransient<IDataLoader     , ConsoleDataLoader>();
        builder.Services.AddTransient<EtlPipeline>();

        return builder.Build();
    }
}