using CommandLine;
using CorpInsightsTW.Core.Enums;
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

    [Option('s', "status", Required = false, Default = "all", 
        HelpText = "目標上市狀態: 'all' (全部), 'listed' (上市公司), 'publicoffering' (公開發行公司)")]
    public string Status { get; set; } = "all";

    [Option('t', "taxonomy", Required = false, Default = "all", 
        HelpText = "目標申報分類法: 'all' (全部), 'general' (一般行業), 'banking' (金融業), 'securities' (證券期貨業), 'holding' (金控業), 'insurance' (保險業), 'crossindustry' (異業別合併)")]
    public string Taxonomy { get; set; } = "all";
}

/// <summary>
/// HttpClient 的控制設定
/// </summary>
public record FetchRunConfig(string Mode, ListingStatus Status, Taxonomy TargetTaxonomy);

public class Program
{    
    public static async Task<int> Main(string[] args)
    {
        var fetchConfig = TryParseConfig(args);
        if (fetchConfig == null) return 1;

        using var host = CreateHost(args, fetchConfig);
        await host.RunAsync();

        return 0;
    }

    /// <summary>
    /// 命令列參數解析與 Core Enums 轉換邏輯
    /// </summary>
    /// <returns>若解析成功回傳組態設定；失敗則回傳 null 且會自動於主控台印出錯誤</returns>
    private static FetchRunConfig? TryParseConfig(string[] args)
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

        if (!Enum.TryParse<Taxonomy>(options.Taxonomy, ignoreCase: true, out var taxonomy))
        {
            Console.WriteLine($"❌ 不合法的申報分類法參數: '{options.Taxonomy}'");
            return null;
        }

        return new FetchRunConfig(options.Mode.ToLower(), status, taxonomy);
    }

    /// <summary>
    /// .NET Host 的初始化與 DI 服務註冊
    /// </summary>
    private static IHost CreateHost(string[] args, FetchRunConfig fetchConfig)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // 給 Worker 與 Job 使用
        builder.Services.AddSingleton(fetchConfig);

        // 註冊核心背景 Worker 與實體作業 Job
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddTransient<FinancialFetchJob>();

        // 註冊標準 HttpClient 工廠
        builder.Services.AddHttpClient();

        return builder.Build();
    }
}
