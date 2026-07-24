using CommandLine;
using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Logging;
using CorpInsightsTW.Core.Storage;
using CorpInsightsTW.DataFetcher.Jobs;
using CorpInsightsTW.DataFetcher.Services;
using Microsoft.Extensions.Logging.Console;

namespace CorpInsightsTW.DataFetcher;

public class Program
{    
    public static async Task<int> Main(string[] args)
    {
        var fetchConfig = TryParseConfig(args);
        if (fetchConfig == null) return 1;

        using var host = CreateHost(args, fetchConfig);
        
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
            var job = host.Services.GetRequiredService<FinancialFetchJob>();

            await job.ExecuteAsync(cts.Token); 

            exitCode = 0;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("🛑 作業已被使用者取消。");
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

    /// <summary>
    /// 命令列參數解析與 Core Enums 轉換邏輯
    /// </summary>
    /// <returns>若解析成功回傳組態設定；失敗則回傳 null 且會自動於主控台印出錯誤</returns>
    private static RuntimeConfig? TryParseConfig(string[] args)
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

        return new RuntimeConfig(status, taxonomy, apCode, string.Empty);
    }

    /// <summary>
    /// .NET Host 的初始化與 DI 服務註冊
    /// </summary>
    private static IHost CreateHost(string[] args, RuntimeConfig runtimeConfig)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options =>
        {
            options.FormatterName = CleanConsoleFormatter.FormatterName; // 指定使用 CleanConsole
        });
        builder.Logging.AddConsoleFormatter<CleanConsoleFormatter, ConsoleFormatterOptions>();

        // Config
        string rootUrl = builder.Configuration["TwseApi:RootUrl"] ?? "https://openapi.twse.com.tw/v1";
        builder.Services.AddSingleton(runtimeConfig with { TwseRootUrl = rootUrl });

        // Storage
        string? customStoragePath = builder.Configuration["Storage:RawDataPath"];
        builder.Services.AddSingleton(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<LocalRawDataStorage>>();
            return new LocalRawDataStorage(logger, customStoragePath);
        });

        builder.Services.AddTransient<FinancialFetchJob>();
        builder.Services.AddTransient<TwseApiService>();

        builder.Services.AddHttpClient();

        return builder.Build();
    }
}
