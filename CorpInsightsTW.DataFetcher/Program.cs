using CommandLine;

using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.DataFetcher.Jobs;
using CorpInsightsTW.DataFetcher.Services;
using CorpInsightsTW.Infrastructure.Storage;

namespace CorpInsightsTW.DataFetcher;

public class Program
{    
    public static async Task<int> Main(string[] args)
    {
        var fetchConfig = TryParseConfig(args);
        if (fetchConfig == null) return 1;

        using var host = CreateHost(args, fetchConfig);
        await host.RunAsync();

        return Environment.ExitCode;
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

        return new FetchRunConfig(status, taxonomy, apCode, string.Empty);
    }

    /// <summary>
    /// .NET Host 的初始化與 DI 服務註冊
    /// </summary>
    private static IHost CreateHost(string[] args, FetchRunConfig fetchConfig)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // 讀取設定檔的 RootUrl
        string rootUrl = builder.Configuration["TwseApi:RootUrl"] ?? "https://openapi.twse.com.tw/v1";
        var finalConfig = fetchConfig with { TwseRootUrl = rootUrl };

        builder.Services.AddSingleton(finalConfig);
        builder.Services.AddSingleton<LocalRawDataStorage>();

        builder.Services.AddHostedService<Worker>();

        builder.Services.AddTransient<FinancialFetchJob>();
        builder.Services.AddTransient<TwseApiService>();

        builder.Services.AddHttpClient();

        return builder.Build();
    }
}
