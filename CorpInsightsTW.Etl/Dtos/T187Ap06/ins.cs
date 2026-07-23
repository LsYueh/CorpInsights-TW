using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos.T187Ap06;

/// <summary>
/// 公司綜合損益表-保險業
/// </summary>
public record InsDto : T187Dto
{
    // 1. 核心識別與索引欄位
    // 繼承自 T187Dto

    // 2. 保險業營業損益項目
    [property: JsonPropertyName("營業收入"), JsonRequired]
    public decimal OperatingRevenue { get; init; } = 0.00m;
    [property: JsonPropertyName("營業成本"), JsonRequired]
    public decimal OperatingCosts { get; init; } = 0.00m;
    [property: JsonPropertyName("營業費用"), JsonRequired]
    public decimal OperatingExpenses { get; init; } = 0.00m;
    [property: JsonPropertyName("營業利益（損失）"), JsonRequired]
    public decimal OperatingIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("營業外收入及支出"), JsonRequired]
    public decimal NonOperatingIncome { get; init; } = 0.00m;

    // 3. 稅前與稅後純益項目
    [property: JsonPropertyName("繼續營業單位稅前純益（純損）"), JsonRequired]
    public decimal IncomeBeforeTax { get; init; } = 0.00m;
    [property: JsonPropertyName("所得稅費用（利益）"), JsonRequired]
    public decimal IncomeTax { get; init; } = 0.00m;
    [property: JsonPropertyName("繼續營業單位本期純益（純損）"), JsonRequired]
    public decimal IncomeAfterTax { get; init; } = 0.00m;
    [property: JsonPropertyName("停業單位損益"), JsonRequired]
    public decimal DiscontinuedOpsIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("合併前非屬共同控制股權損益"), JsonRequired]
    public decimal PreMergerNonControlIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("本期淨利（淨損）"), JsonRequired]
    public decimal NetIncome { get; init; } = 0.00m;

    // 4. 其他綜合損益項目
    [property: JsonPropertyNames(
        "其他綜合損益（淨額）",
        "其他綜合損益（稅後淨額）"), JsonRequired]
    public decimal OtherComprehensiveIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("合併前非屬共同控制股權綜合損益淨額"), JsonRequired]
    public decimal PreMergerNonControlOci { get; init; } = 0.00m;
    [property: JsonPropertyName("本期綜合損益總額"), JsonRequired]
    public decimal TotalComprehensiveIncome { get; init; } = 0.00m;

    // 5. 損益歸屬項目
    [property: JsonPropertyName("淨利（淨損）歸屬於母公司業主"), JsonRequired]
    public decimal NetIncomeParent { get; init; } = 0.00m;
    [property: JsonPropertyName("淨利（淨損）歸屬於共同控制下前手權益"), JsonRequired]
    public decimal NetIncomePredecessor { get; init; } = 0.00m;
    [property: JsonPropertyName("淨利（淨損）歸屬於非控制權益"), JsonRequired]
    public decimal NetIncomeNci { get; init; } = 0.00m;

    // 6. 綜合損益總額歸屬項目
    [property: JsonPropertyName("綜合損益總額歸屬於母公司業主"), JsonRequired]
    public decimal CompIncomeParent { get; init; } = 0.00m;
    [property: JsonPropertyName("綜合損益總額歸屬於共同控制下前手權益"), JsonRequired]
    public decimal CompIncomePredecessor { get; init; } = 0.00m;
    [property: JsonPropertyName("綜合損益總額歸屬於非控制權益"), JsonRequired]
    public decimal CompIncomeNci { get; init; } = 0.00m;

    // 7. 每股盈餘項目
    [property: JsonPropertyName("基本每股盈餘（元）"), JsonRequired]
    public decimal BasicEps { get; init; } = 0.00m;

    // 8. 系統稽核欄位
    public DateTime? UpdatedAt = null;
}