using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公司資產負債表-一般業
/// </summary>
public record CiDto(
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("年度"), JsonRequired]
    short Year,
    [property: JsonPropertyName("季別"), JsonRequired]
    byte Quarter,
    [property: JsonPropertyName("公司代號"), JsonRequired]
    string CompanyCode,
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    string CompanyName,

    // 2. 資產類項目
    [property: JsonPropertyName("流動資產"), JsonRequired]
    decimal CurrentAssets = 0.00m,
    [property: JsonPropertyName("非流動資產"), JsonRequired]
    decimal NonCurrentAssets = 0.00m,
    [property: JsonPropertyNames("資產總額", "資產總計"), JsonRequired]
    decimal TotalAssets = 0.00m,

    // 3. 負債類項目
    [property: JsonPropertyName("流動負債"), JsonRequired]
    decimal CurrentLiabs = 0.00m,
    [property: JsonPropertyName("非流動負債"), JsonRequired]
    decimal NonCurrentLiabs = 0.00m,
    [property: JsonPropertyNames("負債總額", "負債總計"), JsonRequired]
    decimal TotalLiabs = 0.00m,

    // 4. 權益類項目
    [property: JsonPropertyName("股本"), JsonRequired]
    decimal ShareCapital = 0.00m,
    [property: JsonPropertyName("權益─具證券性質之虛擬通貨"), JsonRequired]
    decimal SecurityTokenEquity = 0.00m,
    [property: JsonPropertyName("資本公積"), JsonRequired]
    decimal CapitalSurplus = 0.00m,
    [property: JsonPropertyName("保留盈餘"), JsonRequired]
    decimal RetainedEarnings = 0.00m,
    [property: JsonPropertyName("其他權益"), JsonRequired]
    decimal OtherEquity = 0.00m,
    [property: JsonPropertyName("庫藏股票"), JsonRequired]
    decimal TreasuryStock = 0.00m,
    [property: JsonPropertyName("歸屬於母公司業主之權益合計"), JsonRequired]
    decimal EquityAttributableToOwnersOfParent = 0.00m,
    [property: JsonPropertyName("共同控制下前手權益"), JsonRequired]
    decimal PredecessorInterests = 0.00m,
    [property: JsonPropertyName("合併前非屬共同控制股權"), JsonRequired]
    decimal EquityNotUnderCommonControl = 0.00m,
    [property: JsonPropertyName("非控制權益"), JsonRequired]
    decimal NonControllingInterests = 0.00m,
    [property: JsonPropertyNames("權益總額", "權益總計"), JsonRequired]
    decimal TotalEquity = 0.00m,

    // 5. 股數與每股價值項目
    [property: JsonPropertyNames(
        "待註銷股本股數",
        "待註銷股本股數（單位：股）"
        ), JsonRequired]
    decimal PendingCancellationShares = 0.00m,
    [property: JsonPropertyNames(
        "預收股款（權益項下）之約當發行股數",
        "預收股款（權益項下）之約當發行股數（單位：股）"
        ), JsonRequired]
    decimal PreReceivedSharesEquivalent = 0.00m,
    [property: JsonPropertyNames(
        "母公司暨子公司所持有之母公司庫藏股股數",
        "母公司暨子公司所持有之母公司庫藏股股數（單位：股）"
        ), JsonRequired]
    decimal ParentSubsidiaryTreasuryShares = 0.00m,
    [property: JsonPropertyName("每股參考淨值"), JsonRequired]
    decimal NetValuePerShare = 0.00m,

    // 6. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto
{
    public string ListingStatus { get; set; } = string.Empty;
}