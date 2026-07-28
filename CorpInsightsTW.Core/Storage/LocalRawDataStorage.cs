using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace CorpInsightsTW.Core.Storage;

public class LocalRawDataStorage(
    ILogger<LocalRawDataStorage> logger,
    string? basePath = null)
{
    private readonly ILogger<LocalRawDataStorage> _logger = logger;

    private static string GetIndent(int level) => new(' ', level * 4);

    /// <summary>
    /// 檢查今日是否已經有落地檔案
    /// </summary>
    public bool Exists(StorageContext context, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        string path = GetStoragePath(context, indentLevel);
        bool isExist = File.Exists(path);
        
        _logger.LogDebug("{Indent}🔍 [儲存層] 檔案狀態: {Path} -> 存在: {Result}", indent, path, isExist);
        return isExist;
    }

    /// <summary>
    /// 開啟今日落地檔案的唯讀串流
    /// </summary>
    public FileStream OpenReadableStream(StorageContext context, int indentLevel = 0)
    {
        string filePath = GetStoragePath(context, indentLevel);
        
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
    public FileStream CreateWritableStream(StorageContext context, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);

        string filePath = GetStoragePath(context, indentLevel);
        
        // 自動防呆建立目錄
        EnsureDirectoryExists(context, indentLevel);

        _logger.LogInformation("{Indent}💾 [儲存層] 準備寫入檔案: {Path}", indent, filePath);

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
    public string GetStoragePath(StorageContext context, int indentLevel = 0)
    {
        string dateStr   = context.Date.ToString("yyyyMMdd");
        string subFolder = $"{context.Market.ToCode().ToLower()}";

        string fileName = context.Type switch {
            StatementType.T187AP06 or 
            StatementType.T187AP07 => $"{context.Type.ToCode()}_{context.Status.ToCode()}_{context.Taxonomy.ToCode()}.json",
            StatementType.T163SB20 => $"{context.Type.ToCode()}_{context.Status.ToCode()}.html",
            _ => throw new NotSupportedException($"不支援的報表代號: {context.Type}"),
        };

        string[] rootPath = !string.IsNullOrWhiteSpace(basePath)
            ? [Path.GetFullPath(basePath)]            // 產出: /var/my_custom_path/20260713/twse/t187ap07_L_ci.json
            : [AppContext.BaseDirectory, "raw_data"]; // 產出: [執行檔路徑]/raw_data/20260713/twse/t187ap07_L_ci.json

        string[] paths = [.. rootPath, dateStr, subFolder, fileName];

        return Path.Combine(paths);
    }

    /// <summary>
    /// 刪除指定 context 的落地檔案（用於資料損毀或重新下載時）
    /// </summary>
    /// <returns>若檔案原本存在且成功刪除回傳 true；若檔案原本就不存在則回傳 false</returns>
    public bool Delete(StorageContext context, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        string filePath = GetStoragePath(context, indentLevel);

        try
        {
            if (File.Exists(filePath))
            {
                _logger.LogWarning("{Indent}🗑️ [儲存層] 偵測到需要覆蓋的檔案，正在移除: {Path}", indent, filePath);
                File.Delete(filePath);
                return true;
            }

            _logger.LogDebug("{Indent}ℹ️ [儲存層] 嘗試刪除檔案，但該檔案並不存在: {Path}", indent, filePath);
            return false;
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "{Indent}❌ [儲存層] 無法刪除本地原始檔案： {Path}", indent, filePath);
            return false;
        }
    }

    /// <summary>
    /// 確保實體資料夾存在，若不存在則自動建立
    /// </summary>
    private void EnsureDirectoryExists(StorageContext context, int indentLevel = 0)
    {
        string indent = GetIndent(indentLevel);
        
        string fullPath = GetStoragePath(context, indentLevel);
        string? directoryPath = Path.GetDirectoryName(fullPath);

        if (directoryPath != null && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            _logger.LogInformation("{Indent}📁 [儲存層] 自動建立今日資料夾: {Path}", indent, directoryPath);
        }
    }
}