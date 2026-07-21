using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公司資產負債表-保險業
/// </summary>
public record InsDto(
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("年度"), JsonRequired]
    short Year,
    [property: JsonPropertyName("季別"), JsonRequired]
    byte Quarter,
    [property: JsonPropertyName("公司代號"), JsonRequired]
    string CompanyCode,
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    string CompanyName,

    // 2. 保險業資產類項目
    [property: JsonPropertyName("現金及約當現金"), JsonRequired]
    decimal CashAndCashEquivalents = 0.00m,
    [property: JsonPropertyName("應收款項"), JsonRequired]
    decimal Receivables = 0.00m,
    [property: JsonPropertyName("本期所得稅資產"), JsonRequired]
    decimal CurrentTaxAssets = 0.00m,
    [property: JsonPropertyName("待出售資產"), JsonRequired]
    decimal AssetsHeldForSale = 0.00m,
    [property: JsonPropertyName("待分配予業主之資產（或處分群組）"), JsonRequired]
    decimal AssetsForDistribution = 0.00m,
    [property: JsonPropertyName("透過損益按公允價值衡量之金融資產"), JsonRequired]
    decimal FinancialAssetsAtFvtpl = 0.00m,
    [property: JsonPropertyName("透過其他綜合損益按公允價值衡量之金融資產"), JsonRequired]
    decimal FinancialAssetsAtFvoci = 0.00m,
    [property: JsonPropertyName("按攤銷後成本衡量之金融資產"), JsonRequired]
    decimal FinancialAssetsAtAmortizedCost = 0.00m,
    [property: JsonPropertyName("避險之金融資產"), JsonRequired]
    decimal DerivativeFinancialAssetsForHedging = 0.00m,
    [property: JsonPropertyName("採用權益法之投資－淨額"), JsonRequired]
    decimal InvestmentsUsingEquityMethod = 0.00m,
    [property: JsonPropertyName("其他金融資產－淨額"), JsonRequired]
    decimal OtherFinancialAssets = 0.00m,
    [property: JsonPropertyName("投資性不動產"), JsonRequired]
    decimal InvestmentProperty = 0.00m,
    [property: JsonPropertyName("放款"), JsonRequired]
    decimal Loans = 0.00m,
    [property: JsonPropertyName("保險合約資產"), JsonRequired]
    decimal InsuranceContractAssets = 0.00m,
    [property: JsonPropertyName("再保險合約資產"), JsonRequired]
    decimal ReinsuranceContractAssets = 0.00m,
    [property: JsonPropertyName("不動產及設備"), JsonRequired]
    decimal PropertyAndEquipment = 0.00m,
    [property: JsonPropertyName("使用權資產"), JsonRequired]
    decimal RightOfUseAssets = 0.00m,
    [property: JsonPropertyName("無形資產"), JsonRequired]
    decimal IntangibleAssets = 0.00m,
    [property: JsonPropertyName("遞延所得稅資產"), JsonRequired]
    decimal DeferredTaxAssets = 0.00m,
    [property: JsonPropertyName("其他資產"), JsonRequired]
    decimal OtherAssets = 0.00m,
    [property: JsonPropertyName("分離帳戶保險商品資產"), JsonRequired]
    decimal AssetsOnSeparateAccounts = 0.00m,
    [property: JsonPropertyName("資產總計"), JsonRequired]
    decimal TotalAssets = 0.00m,

    // 3. 保險業負債類項目
    [property: JsonPropertyName("短期債務"), JsonRequired]
    decimal ShortTermDebts = 0.00m,
    [property: JsonPropertyName("應付款項"), JsonRequired]
    decimal Payables = 0.00m,
    [property: JsonPropertyName("本期所得稅負債"), JsonRequired]
    decimal CurrentTaxLiabs = 0.00m,
    [property: JsonPropertyName("與待出售資產直接相關之負債"), JsonRequired]
    decimal LiabsRelatedToAssetsHeldForSale = 0.00m,
    [property: JsonPropertyName("透過損益按公允價值衡量之金融負債"), JsonRequired]
    decimal FinancialLiabsAtFvtpl = 0.00m,
    [property: JsonPropertyName("避險之金融負債"), JsonRequired]
    decimal DerivativeFinancialLiabsForHedging = 0.00m,
    [property: JsonPropertyName("應付債券"), JsonRequired]
    decimal BondsPayable = 0.00m,
    [property: JsonPropertyName("特別股負債"), JsonRequired]
    decimal PreferredStockLiabs = 0.00m,
    [property: JsonPropertyName("負債準備"), JsonRequired]
    decimal Provisions = 0.00m,
    [property: JsonPropertyName("其他金融負債"), JsonRequired]
    decimal OtherFinancialLiabs = 0.00m,
    [property: JsonPropertyName("租賃負債"), JsonRequired]
    decimal LeaseLiabs = 0.00m,
    [property: JsonPropertyName("遞延所得稅負債"), JsonRequired]
    decimal DeferredTaxLiabs = 0.00m,
    [property: JsonPropertyName("其他負債"), JsonRequired]
    decimal OtherLiabs = 0.00m,
    [property: JsonPropertyName("負債總計"), JsonRequired]
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
    [property: JsonPropertyName("權益總計"), JsonRequired]
    decimal TotalEquity = 0.00m,

    // 5. 保險業特有總計項目
    [property: JsonPropertyName("負債及權益總計"), JsonRequired]
    decimal TotalLiabsAndEquity = 0.00m,

    // 6. 股數與每股價值項目
    [property: JsonPropertyName("待註銷股本股數（單位：股）"), JsonRequired]
    decimal PendingCancellationShares = 0.00m,
    [property: JsonPropertyName("預收股款（權益項下）之約當發行股數（單位：股）"), JsonRequired]
    decimal PreReceivedSharesEquivalent = 0.00m,
    [property: JsonPropertyName("母公司暨子公司所持有之母公司庫藏股股數（單位：股）"), JsonRequired]
    decimal ParentSubsidiaryTreasuryShares = 0.00m,
    [property: JsonPropertyName("每股參考淨值"), JsonRequired]
    decimal NetValuePerShare = 0.00m,

    // 7. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto
{
    public string MarketType { get; set; } = string.Empty;
}