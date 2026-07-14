using Microsoft.Extensions.Logging;

using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Infrastructure.Storage;
public class LocalRawDataStorage(
    ILogger<LocalRawDataStorage> logger,
    string? basePath = null)
{
    private readonly ILogger<LocalRawDataStorage> _logger = logger;
    
    private const string BaseFolderName = "raw_data";
    private const string pattern = "yyyyMMdd";

    /// <summary>
    /// 檢查今日是否已經有落地檔案
    /// </summary>
    public bool Exists(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly? date = null)
    {
        var targetDate = date ?? DateOnly.FromDateTime(DateTime.Today);

        string path = GetStoragePath(apCode, status, taxonomy, targetDate);
        bool isExist = File.Exists(path);
        
        _logger.LogDebug("🔍 [儲存層] 檔案狀態: {Path} -> 存在: {Result}", path, isExist);
        return isExist;
    }

    /// <summary>
    /// 開啟今日落地檔案的唯讀串流
    /// </summary>
    public FileStream OpenReadableStream(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly? date = null)
    {
        var targetDate = date ?? DateOnly.FromDateTime(DateTime.Today);

        string filePath = GetStoragePath(apCode, status, taxonomy, targetDate);
        
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
    public FileStream CreateWritableStream(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly? date = null)
    {
        var targetDate = date ?? DateOnly.FromDateTime(DateTime.Today);
        
        string filePath = GetStoragePath(apCode, status, taxonomy, targetDate);
        
        // 自動防呆建立目錄
        EnsureDirectoryExists(apCode, status, taxonomy, targetDate);

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
    /// 檔案路徑
    /// </summary>
    public string GetStoragePath(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly date)
    {
        string dateStr = date.ToString(pattern);
        string fileName = $"{apCode.ToCode()}_{status.ToCode()}_{taxonomy.ToCode()}.json";

        if (!string.IsNullOrWhiteSpace(basePath))
        {
            // 使用者有自訂路徑，直接在該路徑下建立日期資料夾
            // 產出: /var/my_custom_path/20260713/t187ap07_L_ci.json
            return Path.Combine(Path.GetFullPath(basePath), dateStr, fileName);
        }
        else
        {
            // 預設值，退回 AppContext.BaseDirectory 底下的 raw_data
            // 產出: [執行檔路徑]/raw_data/20260713/t187ap07_L_ci.json
            return Path.Combine(AppContext.BaseDirectory, BaseFolderName, dateStr, fileName);
        }
    }

    /// <summary>
    /// 確保實體資料夾存在，若不存在則自動建立
    /// </summary>
    private void EnsureDirectoryExists(T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly date)
    {
        string fullPath = GetStoragePath(apCode, status, taxonomy, date);
        string? directoryPath = Path.GetDirectoryName(fullPath);

        if (directoryPath != null && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            _logger.LogInformation("📁 [資料落地] 自動建立今日資料夾: {Path}", directoryPath);
        }
    }
}