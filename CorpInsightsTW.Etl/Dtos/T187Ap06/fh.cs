using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos.T187Ap06;

/// <summary>
/// 公司綜合損益表-金控業
/// </summary>
public record FhDto : BaseT187Dto
{
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    public string CompanyName { get; init; } = string.Empty;

    // 2. 金控業特有營業淨收益項目
    [property: JsonPropertyName("利息淨收益"), JsonRequired]
    public decimal NetInterestIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("其他收益及費損淨額")] // 上市
    public decimal NetOtherIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("利息以外淨收益"), JsonRequired]
    public decimal NonInterestIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("淨收益"), JsonRequired]
    public decimal NetRevenue { get; init; } = 0.00m;

    // 3. 金控與保險特有成本準備與費用項目
    [property: JsonPropertyName("呆帳費用、承諾及保證責任準備提存"), JsonRequired]
    public decimal BadDebtAndProvision { get; init; } = 0.00m;
    [property: JsonPropertyName("保險負債準備淨變動"), JsonRequired]
    public decimal NetChangeInsuranceLiab { get; init; } = 0.00m;
    [property: JsonPropertyName("營業費用"), JsonRequired]
    public decimal OperatingExpenses { get; init; } = 0.00m;

    // 4. 稅前與稅後淨利項目
    [property: JsonPropertyName("繼續營業單位稅前損益"), JsonRequired]
    public decimal IncomeBeforeTax { get; init; } = 0.00m;
    [property: JsonPropertyName("所得稅（費用）利益")] // 公發
    public decimal IncomeTax { get; init; } = 0.00m;
    [property: JsonPropertyName("繼續營業單位本期淨利（淨損）"), JsonRequired]
    public decimal IncomeAfterTax { get; init; } = 0.00m;
    [property: JsonPropertyName("停業單位損益"), JsonRequired]
    public decimal DiscontinuedOpsIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("本期稅後淨利（淨損）"), JsonRequired]
    public decimal NetIncome { get; init; } = 0.00m;

    // 5. 其他綜合損益項目
    [property: JsonPropertyName("本期其他綜合損益（稅後淨額）"), JsonRequired]
    public decimal OtherComprehensiveIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("本期綜合損益總額"), JsonRequired]
    public decimal TotalComprehensiveIncome { get; init; } = 0.00m;

    // 6. 損益歸屬項目
    [property: JsonPropertyName("淨利（淨損）歸屬於母公司業主"), JsonRequired]
    public decimal NetIncomeParent { get; init; } = 0.00m;
    [property: JsonPropertyName("淨利（淨損）歸屬於共同控制下前手權益"), JsonRequired]
    public decimal NetIncomePredecessor { get; init; } = 0.00m;
    [property: JsonPropertyName("淨利（淨損）歸屬於非控制權益"), JsonRequired]
    public decimal NetIncomeNci { get; init; } = 0.00m;

    // 7. 綜合損益總額歸屬項目
    [property: JsonPropertyName("綜合損益總額歸屬於母公司業主"), JsonRequired]
    public decimal CompIncomeParent { get; init; } = 0.00m;
    [property: JsonPropertyName("綜合損益總額歸屬於共同控制下前手權益"), JsonRequired]
    public decimal CompIncomePredecessor { get; init; } = 0.00m;
    [property: JsonPropertyName("綜合損益總額歸屬於非控制權益"), JsonRequired]
    public decimal CompIncomeNci { get; init; } = 0.00m;

    // 8. 每股盈餘項目
    [property: JsonPropertyName("基本每股盈餘（元）"), JsonRequired]
    public decimal BasicEps { get; init; } = 0.00m;

    // 9. 系統稽核欄位
    public DateTime? UpdatedAt = null;
}