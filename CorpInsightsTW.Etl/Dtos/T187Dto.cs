using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos;

/// <summary>
/// 
/// </summary>
/// <param name="Year"></param>
/// <param name="Quarter"></param>
/// <param name="CompanyCode"></param>
public abstract record T187Dto : IT187Dto
{
    // ====== 核心索引與主鍵欄位 (由基底類別統一標註 JSON 屬性) ======
    [JsonPropertyName("年度"), JsonRequired]
    public short Year { get; init; }
    [JsonPropertyName("季別"), JsonRequired]
    public byte Quarter { get; init; }
    [JsonPropertyName("公司代號"), JsonRequired]
    public string CompanyCode { get; init; } = string.Empty;
    [JsonPropertyName("公司名稱"), JsonRequired]
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