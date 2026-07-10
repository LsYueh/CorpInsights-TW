using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using CorpInsightsTW.Infrastructure.Database;

Console.WriteLine("==================================================");
Console.WriteLine("🚀 CorpInsightsTW 資料庫維運與初始化工具啟動");
Console.WriteLine("==================================================");

// 1. 載入 appsettings.json 組態
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("找不到 ConnectionStrings:DefaultConnection 配置。");

string scriptsPath = configuration["MigrationSettings:DatabaseScriptsPath"] 
    ?? throw new InvalidOperationException("找不到 MigrationSettings:DatabaseScriptsPath 配置。");

bool dropFirst = bool.Parse(configuration["MigrationSettings:DropDatabaseFirst"] ?? "false");

// 2. 實例化 Infrastructure 層的盲刷引擎
// (我們馬上就會在 Infrastructure 專案建立這隻 DatabaseInitializer 程式)
var initializer = new DatabaseInitializer(connectionString, scriptsPath);

try
{
    if (dropFirst)
    {
        Console.WriteLine("⚠️ [警告] 偵測到 DropDatabaseFirst = true，開始執行毀滅性資料庫重置...");
        initializer.DropAndCreateDatabase();
    }

    Console.WriteLine("📂 開始依序掃描 00_、01_、02_ 目錄並盲刷 DDL...");
    await initializer.RunAsync();

    Console.WriteLine("\n🎉 資料庫所有大寬表與索引初始化/驗證成功！");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n❌ 執行初始化時發生致命錯誤：");
    Console.WriteLine(ex.Message);
    Console.ResetColor();
    Environment.Exit(1);
}

Console.WriteLine("==================================================");
