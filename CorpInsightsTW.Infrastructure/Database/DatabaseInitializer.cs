using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
// 💡 註：實務上你可以使用 Dapper、MySqlConnector 或 EF Core 的 DbContext.Database.ExecuteSqlRawAsync
// 這裡先以標準的 ADO.NET (MySqlConnector) 邏輯作為規格範本，確保 Native AOT 相容性
using MySqlConnector; 

namespace CorpInsightsTW.Infrastructure.Database;

public class DatabaseInitializer(string connectionString, string scriptsPath)
{
    private readonly string _connectionString = connectionString;
    private readonly string _scriptsPath = scriptsPath;

    /// <summary>
    /// 毀滅性重置：刪除舊資料庫並重建 (本機開發除錯常用)
    /// </summary>
    public void DropAndCreateDatabase()
    {
        // 先用不需要指定 Database 的連線字串連進 MySQL
        var builder = new MySqlConnectionStringBuilder(_connectionString);
        string targetDatabase = builder.Database;
        builder.Database = ""; // 清空資料庫名稱，先連到伺服器層級

        using var conn = new MySqlConnection(builder.ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"DROP DATABASE IF EXISTS `{targetDatabase}`; CREATE DATABASE `{targetDatabase}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
        cmd.ExecuteNonQuery();
        
        Console.WriteLine($"   🔥 資料庫 `{targetDatabase}` 已被徹底抹除並重新建立空庫。");
    }

    /// <summary>
    /// 核心盲刷邏輯：依 00_, 01_, 02_ 順序遍歷並執行 SQL
    /// </summary>
    public async Task RunAsync()
    {
        // 確保路徑存在
        string absoluteScriptsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _scriptsPath));
        if (!Directory.Exists(absoluteScriptsPath))
        {
            throw new DirectoryNotFoundException($"找不到 SQL 腳本根目錄：{absoluteScriptsPath}");
        }

        // 1. 取得所有子資料夾，並依名稱排序 (保證 00 -> 01 -> 02 執行順序)
        var directories = Directory.GetDirectories(absoluteScriptsPath)
                                   .Select(d => new DirectoryInfo(d))
                                   .OrderBy(d => d.Name);

        using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();

        foreach (var dir in directories)
        {
            Console.WriteLine($"\n📁 正在處理目錄: {dir.Name}");

            // 2. 取得該目錄下所有的 .sql 檔案，依檔名排序
            var sqlFiles = dir.GetFiles("*.sql").OrderBy(f => f.Name);

            foreach (var file in sqlFiles)
            {
                Console.Write($"   📄 執行 {file.Name} ... ");

                // 3. 讀取 SQL 內容
                string sqlContent = await File.ReadAllTextAsync(file.FullName);

                if (string.IsNullOrWhiteSpace(sqlContent))
                {
                    Console.WriteLine("跳過 (空檔案)");
                    continue;
                }

                // 4. 丟給 MySQL 執行
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sqlContent;
                
                // 設定超時時間長一點，避免大寬表加索引時卡住
                cmd.CommandTimeout = 180; 

                await cmd.ExecuteNonQueryAsync();
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("成功");
                Console.ResetColor();
            }
        }
    }
}