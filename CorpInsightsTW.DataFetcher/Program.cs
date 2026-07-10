using CommandLine;
using CorpInsightsTW.DataFetcher.Jobs;

namespace CorpInsightsTW.DataFetcher;

/// <summary>
/// CommandLineParser 專用宣告式設定類別
/// </summary>
public class CliOptions
{
    [Option('m', "mode", Required = false, Default = "once", 
        HelpText = "執行模式: 'once' (單次抓取後關閉) 或 'daemon' (常駐背景排程)")]
    public string Mode { get; set; } = "once";

    [Option('i', "industry", Required = false, Default = "all", 
        HelpText = "目標產業過濾: 'all' (全量), 'ci' (一般產業), 'fh' (金控業)")]
    public string Industry { get; set; } = "all";
}

/// <summary>
/// 全專案共享的強型態控制配置
/// </summary>
public record FetchRunConfig(string Mode, string Industry);

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // ========================================================
        // 1. 使用 CommandLineParser 進行強型態參數解析
        // ========================================================
        var parseResult = Parser.Default.ParseArguments<CliOptions>(args);

        // 如果解析失敗（例如輸入 --help 或錯誤參數），套件會自動印出說明並結束程式
        if (parseResult.Tag == ParserResultType.NotParsed)
        {
            return 1;
        }

        // 解析成功，提取強型態數值
        var options = ((Parsed<CliOptions>)parseResult).Value;

        // ========================================================
        // 2. 啟動 .NET 相依性注入中央廚房
        // ========================================================
        var builder = Host.CreateApplicationBuilder(args);

        // 將解析後的配置封裝為單例，注入給 Worker 與 Job 使用
        var fetchConfig = new FetchRunConfig(options.Mode.ToLower(), options.Industry.ToLower());
        builder.Services.AddSingleton(fetchConfig);

        // 註冊核心背景 Worker 與實體作業 Job
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddTransient<FinancialFetchJob>();

        var host = builder.Build();
        await host.RunAsync();

        return 0;
    }
}
