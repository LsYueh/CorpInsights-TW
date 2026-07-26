using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos;

public abstract record T187Dto : IT187Dto
{
    [JsonPropertyNames("出表日期", "Date"), JsonRequired]
    public required string ReportDate { get; init; }
    
    // ====== 核心索引與主鍵欄位 (由基底類別統一標註 JSON 屬性) ======
    [JsonPropertyNames("年度", "Year"), JsonRequired]
    public short Year { get; init; }
    [JsonPropertyNames("季別", "Season"), JsonRequired]
    public byte Quarter { get; init; }
    [JsonPropertyNames("公司代號", "SecuritiesCompanyCode"), JsonRequired]
    public string CompanyCode { get; init; } = string.Empty;
    [JsonPropertyNames("公司名稱", "CompanyName"), JsonRequired]
    public string CompanyName { get; init; } = string.Empty;

    // ====== ETL 自行加工欄位 ======
    public string ListingStatus { get; set; } = string.Empty;

    public virtual bool IsValidKey()
    {
        if (string.IsNullOrWhiteSpace(CompanyCode))
            return false;

        if (Year <= 0)
            return false;

        if (Quarter is < 1 or > 4)
            return false;

        return true;
    }
}