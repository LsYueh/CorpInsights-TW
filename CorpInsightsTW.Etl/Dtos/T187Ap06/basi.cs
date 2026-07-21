using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos.T187Ap06;

/// <summary>
/// 公司綜合損益表-金融業
/// </summary>
public record BasiDto
(
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("年度"), JsonRequired]
    int Year,
    [property: JsonPropertyName("季別"), JsonRequired]
    int Quarter,
    [property: JsonPropertyName("公司代號"), JsonRequired]
    string CompanyCode,
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    string CompanyName,

    // 2. 金融業營業損益項目
    [property: JsonPropertyName("利息淨收益"), JsonRequired]
    decimal InterestIncome = 0.00m,
    [property: JsonPropertyName("利息以外淨損益"), JsonRequired]
    decimal NonInterestIncome = 0.00m,
    [property: JsonPropertyName("呆帳費用、承諾及保證責任準備提存"), JsonRequired]
    decimal ProvisionsExpenses = 0.00m,
    [property: JsonPropertyName("營業費用"), JsonRequired]
    decimal OperatingExpenses = 0.00m,

    // 3. 稅前與稅後淨利項目
    [property: JsonPropertyName("繼續營業單位稅前淨利（淨損）"), JsonRequired]
    decimal IncomeBeforeTax = 0.00m,
    [property: JsonPropertyName("所得稅費用（利益）"), JsonRequired]
    decimal IncomeTax = 0.00m,
    [property: JsonPropertyNames(
        "繼續營業單位本期淨利（淨損）",
        "繼續營業單位本期稅後淨利（淨損）"), JsonRequired]
    decimal IncomeAfterTax = 0.00m,
    [property: JsonPropertyName("停業單位損益"), JsonRequired]
    decimal DiscontinuedOpsIncome = 0.00m,
    [property: JsonPropertyName("合併前非屬共同控制股權損益"), JsonRequired]
    decimal PreMergerNonControlIncome = 0.00m,
    [property: JsonPropertyNames("本期淨利（淨損）", "本期稅後淨利（淨損）"), JsonRequired]
    decimal NetIncome = 0.00m,

    // 4. 其他綜合損益項目
    [property: JsonPropertyName("其他綜合損益（稅後）"), JsonRequired]
    decimal OtherComprehensiveIncome = 0.00m,
    [property: JsonPropertyName("合併前非屬共同控制股權綜合損益淨額"), JsonRequired]
    decimal PreMergerNonControlOci = 0.00m,
    [property: JsonPropertyName("本期綜合損益總額（稅後）"), JsonRequired]
    decimal TotalComprehensiveIncome = 0.00m,

    // 5. 損益歸屬項目
    [property: JsonPropertyName("淨利（損）歸屬於母公司業主"), JsonRequired]
    decimal NetIncomeParent = 0.00m,
    [property: JsonPropertyName("淨利（損）歸屬於共同控制下前手權益"), JsonRequired]
    decimal NetIncomePredecessor = 0.00m,
    [property: JsonPropertyName("淨利（損）歸屬於非控制權益"), JsonRequired]
    decimal NetIncomeNci = 0.00m,

    // 6. 綜合損益總額歸屬項目
    [property: JsonPropertyName("綜合損益總額歸屬於母公司業主"), JsonRequired]
    decimal CompIncomeParent = 0.00m,
    [property: JsonPropertyName("綜合損益總額歸屬於共同控制下前手權益"), JsonRequired]
    decimal CompIncomePredecessor = 0.00m,
    [property: JsonPropertyName("綜合損益總額歸屬於非控制權益"), JsonRequired]
    decimal CompIncomeNci = 0.00m,

    // 7. 每股盈餘項目
    [property: JsonPropertyName("基本每股盈餘（元）"), JsonRequired]
    decimal BasicEps = 0.00m,

    // 8. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto
{
    public string ListingStatus { get; set; } = string.Empty;
}