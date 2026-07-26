using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos.T187Ap06;

/// <summary>
/// 公司綜合損益表-金融業
/// </summary>
public record BasiDto : T187Dto
{
    // 1. 核心識別與索引欄位
    // 繼承自 T187Dto

    // 2. 金融業營業損益項目
    [JsonPropertyName("利息淨收益"), JsonRequired]
    public decimal InterestIncome { get; init; } = 0.00m;
    [JsonPropertyName("利息以外淨損益"), JsonRequired]
    public decimal NonInterestIncome { get; init; } = 0.00m;
    [JsonPropertyName("呆帳費用、承諾及保證責任準備提存"), JsonRequired]
    public decimal ProvisionsExpenses { get; init; } = 0.00m;
    [JsonPropertyName("營業費用"), JsonRequired]
    public decimal OperatingExpenses { get; init; } = 0.00m;

    // 3. 稅前與稅後淨利項目
    [JsonPropertyName("繼續營業單位稅前淨利（淨損）"), JsonRequired]
    public decimal IncomeBeforeTax { get; init; } = 0.00m;
    [JsonPropertyName("所得稅費用（利益）"), JsonRequired]
    public decimal IncomeTax { get; init; } = 0.00m;
    [JsonPropertyNames(
        "繼續營業單位本期淨利（淨損）",
        "繼續營業單位本期稅後淨利（淨損）"), JsonRequired]
    public decimal IncomeAfterTax { get; init; } = 0.00m;
    [JsonPropertyName("停業單位損益"), JsonRequired]
    public decimal DiscontinuedOpsIncome { get; init; } = 0.00m;
    [JsonPropertyName("合併前非屬共同控制股權損益"), JsonRequired]
    public decimal PreMergerNonControlIncome { get; init; } = 0.00m;
    [JsonPropertyNames("本期淨利（淨損）", "本期稅後淨利（淨損）"), JsonRequired]
    public decimal NetIncome { get; init; } = 0.00m;

    // 4. 其他綜合損益項目
    [JsonPropertyName("其他綜合損益（稅後）"), JsonRequired]
    public decimal OtherComprehensiveIncome { get; init; } = 0.00m;
    [JsonPropertyName("合併前非屬共同控制股權綜合損益淨額"), JsonRequired]
    public decimal PreMergerNonControlOci { get; init; } = 0.00m;
    [JsonPropertyName("本期綜合損益總額（稅後）"), JsonRequired]
    public decimal TotalComprehensiveIncome { get; init; } = 0.00m;

    // 5. 損益歸屬項目
    [JsonPropertyName("淨利（損）歸屬於母公司業主"), JsonRequired]
    public decimal NetIncomeParent { get; init; } = 0.00m;
    [JsonPropertyName("淨利（損）歸屬於共同控制下前手權益"), JsonRequired]
    public decimal NetIncomePredecessor { get; init; } = 0.00m;
    [JsonPropertyName("淨利（損）歸屬於非控制權益"), JsonRequired]
    public decimal NetIncomeNci { get; init; } = 0.00m;

    // 6. 綜合損益總額歸屬項目
    [JsonPropertyName("綜合損益總額歸屬於母公司業主"), JsonRequired]
    public decimal CompIncomeParent { get; init; } = 0.00m;
    [JsonPropertyName("綜合損益總額歸屬於共同控制下前手權益"), JsonRequired]
    public decimal CompIncomePredecessor { get; init; } = 0.00m;
    [JsonPropertyName("綜合損益總額歸屬於非控制權益"), JsonRequired]
    public decimal CompIncomeNci { get; init; } = 0.00m;

    // 7. 每股盈餘項目
    [JsonPropertyName("基本每股盈餘（元）"), JsonRequired]
    public decimal BasicEps { get; init; } = 0.00m;

    // 8. 系統稽核欄位
    public DateTime? UpdatedAt = null;
}