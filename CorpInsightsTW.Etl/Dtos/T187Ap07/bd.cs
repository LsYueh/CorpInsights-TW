using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公司資產負債表-證券期貨業
/// </summary>
public record BdDto : BaseT187Dto
{
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    public string CompanyName { get; init; } = string.Empty;

    // 2. 資產類項目
    [property: JsonPropertyName("流動資產"), JsonRequired]
    public decimal CurrentAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("非流動資產"), JsonRequired]
    public decimal NonCurrentAssets { get; init; } = 0.00m;
    [property: JsonPropertyNames("資產總額", "資產總計"), JsonRequired]
    public decimal TotalAssets { get; init; } = 0.00m;

    // 3. 負債類項目
    [property: JsonPropertyName("流動負債"), JsonRequired]
    public decimal CurrentLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("非流動負債"), JsonRequired]
    public decimal NonCurrentLiabs { get; init; } = 0.00m;
    [property: JsonPropertyNames("負債總額", "負債總計"), JsonRequired]
    public decimal TotalLiabs { get; init; } = 0.00m;

    // 4. 權益類項目
    [property: JsonPropertyName("股本"), JsonRequired]
    public decimal ShareCapital { get; init; } = 0.00m;
    [property: JsonPropertyName("權益－具證券性質之虛擬通貨"), JsonRequired]
    public decimal SecurityTokenEquity { get; init; } = 0.00m;
    [property: JsonPropertyName("資本公積"), JsonRequired]
    public decimal CapitalSurplus { get; init; } = 0.00m;
    [property: JsonPropertyNames("保留盈餘", "保留盈餘（或累積虧損）"), JsonRequired]
    public decimal RetainedEarnings { get; init; } = 0.00m;
    [property: JsonPropertyName("其他權益"), JsonRequired]
    public decimal OtherEquity { get; init; } = 0.00m;
    [property: JsonPropertyName("庫藏股票"), JsonRequired]
    public decimal TreasuryStock { get; init; } = 0.00m;
    [property: JsonPropertyNames(
        "歸屬於母公司業主權益合計",
        "歸屬於母公司業主之權益合計"
        ), JsonRequired]
    public decimal EquityAttributableToOwnersOfParent { get; init; } = 0.00m;
    [property: JsonPropertyName("共同控制下前手權益"), JsonRequired]
    public decimal PredecessorInterests { get; init; } = 0.00m;
    [property: JsonPropertyName("合併前非屬共同控制股權"), JsonRequired]
    public decimal EquityNotUnderCommonControl { get; init; } = 0.00m;
    [property: JsonPropertyName("非控制權益"), JsonRequired]
    public decimal NonControllingInterests { get; init; } = 0.00m;
    [property: JsonPropertyNames("權益總額", "權益總計"), JsonRequired]
    public decimal TotalEquity { get; init; } = 0.00m;

    // 5. 股數與每股價值項目
    [property: JsonPropertyNames(
        "待註銷股本股數",
        "待註銷股本股數（單位：股）"
        ), JsonRequired]
    public decimal PendingCancellationShares { get; init; } = 0.00m;
    [property: JsonPropertyNames(
        "預收股款（權益項下）之約當發行股數",
        "預收股款（權益項下）之約當發行股數（單位：股）"
        ), JsonRequired]
    public decimal PreReceivedSharesEquivalent { get; init; } = 0.00m;
    [property: JsonPropertyNames(
        "母公司暨子公司所持有之母公司庫藏股股數",
        "母公司暨子公司持有之母公司庫藏股股數（單位：股）"
        ), JsonRequired]
    public decimal ParentSubsidiaryTreasuryShares { get; init; } = 0.00m;
    [property: JsonPropertyName("每股參考淨值"), JsonRequired]
    public decimal NetValuePerShare { get; init; } = 0.00m;

    // 6. 系統稽核欄位
    public DateTime? UpdatedAt = null;
}