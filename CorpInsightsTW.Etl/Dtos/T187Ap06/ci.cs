using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos.T187Ap06;

/// <summary>
/// 公司綜合損益表-一般業
/// </summary>
public record CiDto : BaseT187Dto
{
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    public string CompanyName { get; init; } = string.Empty;

    // 2. 營業收入、成本與毛利項目
    [property: JsonPropertyName("營業收入"), JsonRequired]
    public decimal Revenue { get; init; } = 0.00m;
    [property: JsonPropertyName("營業成本"), JsonRequired]
    public decimal Costs { get; init; } = 0.00m;
    [property: JsonPropertyName("原始認列生物資產及農產品之利益（損失）"), JsonRequired]
    public decimal BioAssetsInitialGainLoss { get; init; } = 0.00m;
    [property: JsonPropertyName("生物資產當期公允價值減出售成本之變動利益（損失）"), JsonRequired]
    public decimal BioAssetsFvGainLoss { get; init; } = 0.00m;
    [property: JsonPropertyName("營業毛利（毛損）"), JsonRequired]
    public decimal GrossProfit { get; init; } = 0.00m;
    [property: JsonPropertyName("未實現銷貨（損）益"), JsonRequired]
    public decimal UnrealizedGainsLosses { get; init; } = 0.00m;
    [property: JsonPropertyName("已實現銷貨（損）益"), JsonRequired]
    public decimal RealizedGainsLosses { get; init; } = 0.00m;
    [property: JsonPropertyName("營業毛利（毛損）淨額"), JsonRequired]
    public decimal NetGrossProfit { get; init; } = 0.00m;

    // 3. 營業費用與營業利益
    [property: JsonPropertyName("營業費用"), JsonRequired]
    public decimal Expenses { get; init; } = 0.00m;
    [property: JsonPropertyName("其他收益及費損淨額"), JsonRequired]
    public decimal NetOtherIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("營業利益（損失）"), JsonRequired]
    public decimal OperatingIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("營業外收入及支出"), JsonRequired]
    public decimal NonOperatingIncome { get; init; } = 0.00m;

    // 4. 稅前與稅後淨利項目
    [property: JsonPropertyName("稅前淨利（淨損）"), JsonRequired]
    public decimal IncomeBeforeTax { get; init; } = 0.00m;
    [property: JsonPropertyName("所得稅費用（利益）"), JsonRequired]
    public decimal IncomeTax { get; init; } = 0.00m;
    [property: JsonPropertyName("繼續營業單位本期淨利（淨損）"), JsonRequired]
    public decimal IncomeAfterTax { get; init; } = 0.00m;
    [property: JsonPropertyName("停業單位損益"), JsonRequired]
    public decimal DiscontinuedOpsIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("合併前非屬共同控制股權損益"), JsonRequired]
    public decimal PreMergerNonControlIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("本期淨利（淨損）"), JsonRequired]
    public decimal NetIncome { get; init; } = 0.00m;

    // 5. 其他綜合損益項目
    [property: JsonPropertyName("其他綜合損益（淨額）"), JsonRequired]
    public decimal OtherComprehensiveIncome { get; init; } = 0.00m;
    [property: JsonPropertyName("合併前非屬共同控制股權綜合損益淨額"), JsonRequired]
    public decimal PreMergerNonControlOci { get; init; } = 0.00m;
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