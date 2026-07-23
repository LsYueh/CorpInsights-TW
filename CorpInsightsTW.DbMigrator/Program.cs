using CorpInsightsTW.Core.Logging;
using CorpInsightsTW.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MySqlConnector;

namespace CorpInsightsTW.DbMigrator;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var host = CreateHost(args);
        var logger        = host.Services.GetRequiredService<ILogger<Program>>();
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        
        logger.LogInformation("==================================================");
        logger.LogInformation("🚀 CorpInsightsTW CIS 資料庫初始化工具啟動");
        logger.LogInformation("==================================================");

        string connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("找不到 ConnectionStrings:DefaultConnection 配置。");

        string scriptsPath = configuration["MigrationSettings:DatabaseScriptsPath"] 
            ?? throw new InvalidOperationException("找不到 MigrationSettings:DatabaseScriptsPath 配置。");

        bool dropFirst = bool.Parse(configuration["MigrationSettings:DropDatabaseFirst"] ?? "false");
   
        var csb = new MySqlConnectionStringBuilder(connectionString);
        logger.LogInformation("🔌 連線目標   : {Server} / {Database}", csb.Server, csb.Database);
        logger.LogInformation("📁 腳本路徑   : {ScriptsPath}", scriptsPath);
        logger.LogInformation("🗑️ 重置資料庫 : {DropFirst}", dropFirst);

        try
        {
            var initializer = new DatabaseInitializer(connectionString, scriptsPath);

            if (dropFirst)
            {
                logger.LogWarning("⚠️ [警告] 偵測到 DropDatabaseFirst = true ，開始執行毀滅性資料庫重置...");
                initializer.DropAndCreateDatabase();
            }

            logger.LogInformation("📂 開始依序掃描 00_、01_、02_ 目錄並執行 DDL...");
            await initializer.RunAsync();

            logger.LogInformation("🎉 資料庫所有大寬表與索引初始化/驗證成功！");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ 執行初始化時發生致命錯誤");
            return 1;
        }

        logger.LogInformation("==================================================");
        return 0;
    }

    private static IHost CreateHost(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options =>
        {
            options.FormatterName = CleanConsoleFormatter.FormatterName; // 指定使用 CleanConsole
        });
        builder.Logging.AddConsoleFormatter<CleanConsoleFormatter, ConsoleFormatterOptions>();

        return builder.Build();
    }
}
