using CommandLine;

using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Etl;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var runConfig = TryParseConfig(args);
        if (runConfig == null) return 1;

        using var host = CreateHost(args, runConfig);

        // var pipeline = host.Services.GetRequiredService<EtlPipeline>();
        // await pipeline.RunAsync();

        return 0;
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

        builder.Services.AddSingleton(runConfig);

        // builder.Services.AddSingleton<IDataExtractor, LocalRawDataExtractor>();
        // builder.Services.AddSingleton<IDataTransformer, PassthroughDataTransformer>();
        // builder.Services.AddSingleton<IDataLoader, ConsoleDataLoader>();
        // builder.Services.AddSingleton<EtlPipeline>();

        return builder.Build();
    }
}