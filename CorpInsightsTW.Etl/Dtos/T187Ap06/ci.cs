using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos.T187Ap06;

/// <summary>
/// 公司綜合損益表-一般業
/// </summary>
public record CiDto
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

    // 2. 營業收入、成本與毛利項目
    [property: JsonPropertyName("營業收入"), JsonRequired]
    decimal Revenue = 0.00m,
    [property: JsonPropertyName("營業成本"), JsonRequired]
    decimal Costs = 0.00m,
    [property: JsonPropertyName("原始認列生物資產及農產品之利益（損失）"), JsonRequired]
    decimal BioAssetsInitialGainLoss = 0.00m,
    [property: JsonPropertyName("生物資產當期公允價值減出售成本之變動利益（損失）"), JsonRequired]
    decimal BioAssetsFvGainLoss = 0.00m,
    [property: JsonPropertyName("營業毛利（毛損）"), JsonRequired]
    decimal GrossProfit = 0.00m,
    [property: JsonPropertyName("未實現銷貨（損）益"), JsonRequired]
    decimal UnrealizedGainsLosses = 0.00m,
    [property: JsonPropertyName("已實現銷貨（損）益"), JsonRequired]
    decimal RealizedGainsLosses = 0.00m,
    [property: JsonPropertyName("營業毛利（毛損）淨額"), JsonRequired]
    decimal NetGrossProfit = 0.00m,

    // 3. 營業費用與營業利益
    [property: JsonPropertyName("營業費用"), JsonRequired]
    decimal Expenses = 0.00m,
    [property: JsonPropertyName("其他收益及費損淨額"), JsonRequired]
    decimal NetOtherIncome = 0.00m,
    [property: JsonPropertyName("營業利益（損失）"), JsonRequired]
    decimal OperatingIncome = 0.00m,
    [property: JsonPropertyName("營業外收入及支出"), JsonRequired]
    decimal NonOperatingIncome = 0.00m,

    // 4. 稅前與稅後淨利項目
    [property: JsonPropertyName("稅前淨利（淨損）"), JsonRequired]
    decimal IncomeBeforeTax = 0.00m,
    [property: JsonPropertyName("所得稅費用（利益）"), JsonRequired]
    decimal IncomeTax = 0.00m,
    [property: JsonPropertyName("繼續營業單位本期淨利（淨損）"), JsonRequired]
    decimal IncomeAfterTax = 0.00m,
    [property: JsonPropertyName("停業單位損益"), JsonRequired]
    decimal DiscontinuedOpsIncome = 0.00m,
    [property: JsonPropertyName("合併前非屬共同控制股權損益"), JsonRequired]
    decimal PreMergerNonControlIncome = 0.00m,
    [property: JsonPropertyName("本期淨利（淨損）"), JsonRequired]
    decimal NetIncome = 0.00m,

    // 5. 其他綜合損益項目
    [property: JsonPropertyName("其他綜合損益（稅後）"), JsonRequired]
    decimal OtherComprehensiveIncome = 0.00m,
    [property: JsonPropertyName("合併前非屬共同控制股權綜合損益淨額"), JsonRequired]
    decimal PreMergerNonControlOci = 0.00m,
    [property: JsonPropertyName("本期綜合損益總額（稅後）"), JsonRequired]
    decimal TotalComprehensiveIncome = 0.00m,

    // 6. 損益歸屬項目
    [property: JsonPropertyName("淨利（淨損）歸屬於母公司業主"), JsonRequired]
    decimal NetIncomeParent = 0.00m,
    [property: JsonPropertyName("淨利（淨損）歸屬於共同控制下前手權益"), JsonRequired]
    decimal NetIncomePredecessor = 0.00m,
    [property: JsonPropertyName("淨利（淨損）歸屬於非控制權益"), JsonRequired]
    decimal NetIncomeNci = 0.00m,

    // 7. 綜合損益總額歸屬項目
    [property: JsonPropertyName("綜合損益總額歸屬於母公司業主"), JsonRequired]
    decimal CompIncomeParent = 0.00m,
    [property: JsonPropertyName("綜合損益總額歸屬於共同控制下前手權益"), JsonRequired]
    decimal CompIncomePredecessor = 0.00m,
    [property: JsonPropertyName("綜合損益總額歸屬於非控制權益"), JsonRequired]
    decimal CompIncomeNci = 0.00m,

    // 8. 每股盈餘項目
    [property: JsonPropertyName("基本每股盈餘（元）"), JsonRequired]
    decimal BasicEps = 0.00m,

    // 9. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto
{
    public string MarketType { get; set; } = string.Empty;
}