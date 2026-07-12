using Microsoft.Extensions.Logging;

using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Infrastructure.Storage;
public class LocalRawDataStorage(
    ILogger<LocalRawDataStorage> logger)
{
    private readonly ILogger<LocalRawDataStorage> _logger = logger;
    private const string BaseFolderName = "raw_data";

    private const string pattern = "yyyyMMdd";

    /// <summary>
    /// 檢查今日是否已經有落地檔案
    /// </summary>
    public static bool Exists(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy)
    {
        string path = GetStoragePath(apCode, status, taxonomy);
        return File.Exists(path);
    }

    /// <summary>
    /// 開啟今日落地檔案的唯讀串流
    /// </summary>
    public static FileStream OpenReadableStream(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy)
    {
        string filePath = GetStoragePath(apCode, status, taxonomy);
        
        return new FileStream(
            filePath, 
            FileMode.Open, 
            FileAccess.Read, 
            FileShare.Read, 
            bufferSize: 4096, 
            useAsync: true);
    }

    /// <summary>
    /// 自動建立目錄，並開啟/滾動一個全新的可寫入 FileStream
    /// </summary>
    public FileStream CreateWritableStream(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy)
    {
        string filePath = GetStoragePath(apCode, status, taxonomy);
        
        // 自動防呆建立目錄
        EnsureDirectoryExists();

        _logger.LogInformation("💾 [資料落地] 準備寫入檔案: {Path}", filePath);

        return new FileStream(
            filePath, 
            FileMode.Create, 
            FileAccess.Write, 
            FileShare.None, 
            bufferSize: 4096, 
            useAsync: true);
    }

    /// <summary>
    /// 計算強型態的兩層式檔案路徑
    /// </summary>
    private static string GetStoragePath(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy)
    {
        string todayStr = DateTime.Today.ToString(pattern);
        string fileName = $"{apCode.ToCode()}_{status.ToCode()}_{taxonomy.ToCode()}.json";
        
        // 產出如: raw_data/20260712/t187ap07_L_ci.json
        return Path.Combine(AppContext.BaseDirectory, BaseFolderName, todayStr, fileName);
    }

    /// <summary>
    /// 確保實體資料夾存在，若不存在則自動建立
    /// </summary>
    private void EnsureDirectoryExists()
    {
        string todayStr = DateTime.Today.ToString(pattern);
        string directoryPath = Path.Combine(AppContext.BaseDirectory, BaseFolderName, todayStr);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            _logger.LogInformation("📁 [資料落地] 自動建立今日資料夾: {Path}", directoryPath);
        }
    }
}