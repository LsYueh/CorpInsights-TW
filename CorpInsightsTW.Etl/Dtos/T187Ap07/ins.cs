using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公司資產負債表-保險業
/// </summary>
public record InsDto : BaseT187Dto
{
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    public string CompanyName { get; init; } = string.Empty;

    // 2. 保險業資產類項目
    [property: JsonPropertyName("現金及約當現金"), JsonRequired]
    public decimal CashAndCashEquivalents { get; init; } = 0.00m;
    [property: JsonPropertyName("應收款項"), JsonRequired]
    public decimal Receivables { get; init; } = 0.00m;
    [property: JsonPropertyName("本期所得稅資產"), JsonRequired]
    public decimal CurrentTaxAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("待出售資產"), JsonRequired]
    public decimal AssetsHeldForSale { get; init; } = 0.00m;
    [property: JsonPropertyName("待分配予業主之資產（或處分群組）"), JsonRequired]
    public decimal AssetsForDistribution { get; init; } = 0.00m;
    [property: JsonPropertyName("透過損益按公允價值衡量之金融資產"), JsonRequired]
    public decimal FinancialAssetsAtFvtpl { get; init; } = 0.00m;
    [property: JsonPropertyName("透過其他綜合損益按公允價值衡量之金融資產"), JsonRequired]
    public decimal FinancialAssetsAtFvoci { get; init; } = 0.00m;
    [property: JsonPropertyName("按攤銷後成本衡量之金融資產"), JsonRequired]
    public decimal FinancialAssetsAtAmortizedCost { get; init; } = 0.00m;
    [property: JsonPropertyName("避險之金融資產"), JsonRequired]
    public decimal DerivativeFinancialAssetsForHedging { get; init; } = 0.00m;
    [property: JsonPropertyName("採用權益法之投資－淨額"), JsonRequired]
    public decimal InvestmentsUsingEquityMethod { get; init; } = 0.00m;
    [property: JsonPropertyName("其他金融資產－淨額"), JsonRequired]
    public decimal OtherFinancialAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("投資性不動產"), JsonRequired]
    public decimal InvestmentProperty { get; init; } = 0.00m;
    [property: JsonPropertyName("放款"), JsonRequired]
    public decimal Loans { get; init; } = 0.00m;
    [property: JsonPropertyName("保險合約資產"), JsonRequired]
    public decimal InsuranceContractAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("再保險合約資產"), JsonRequired]
    public decimal ReinsuranceContractAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("不動產及設備"), JsonRequired]
    public decimal PropertyAndEquipment { get; init; } = 0.00m;
    [property: JsonPropertyName("使用權資產"), JsonRequired]
    public decimal RightOfUseAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("無形資產"), JsonRequired]
    public decimal IntangibleAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("遞延所得稅資產"), JsonRequired]
    public decimal DeferredTaxAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("其他資產"), JsonRequired]
    public decimal OtherAssets { get; init; } = 0.00m;
    [property: JsonPropertyName("分離帳戶保險商品資產"), JsonRequired]
    public decimal AssetsOnSeparateAccounts { get; init; } = 0.00m;
    [property: JsonPropertyName("資產總計"), JsonRequired]
    public decimal TotalAssets { get; init; } = 0.00m;

    // 3. 保險業負債類項目
    [property: JsonPropertyName("短期債務"), JsonRequired]
    public decimal ShortTermDebts { get; init; } = 0.00m;
    [property: JsonPropertyName("應付款項"), JsonRequired]
    public decimal Payables { get; init; } = 0.00m;
    [property: JsonPropertyName("本期所得稅負債"), JsonRequired]
    public decimal CurrentTaxLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("與待出售資產直接相關之負債"), JsonRequired]
    public decimal LiabsRelatedToAssetsHeldForSale { get; init; } = 0.00m;
    [property: JsonPropertyName("透過損益按公允價值衡量之金融負債"), JsonRequired]
    public decimal FinancialLiabsAtFvtpl { get; init; } = 0.00m;
    [property: JsonPropertyName("避險之金融負債"), JsonRequired]
    public decimal DerivativeFinancialLiabsForHedging { get; init; } = 0.00m;
    [property: JsonPropertyName("應付債券"), JsonRequired]
    public decimal BondsPayable { get; init; } = 0.00m;
    [property: JsonPropertyName("特別股負債"), JsonRequired]
    public decimal PreferredStockLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("負債準備"), JsonRequired]
    public decimal Provisions { get; init; } = 0.00m;
    [property: JsonPropertyName("其他金融負債"), JsonRequired]
    public decimal OtherFinancialLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("租賃負債"), JsonRequired]
    public decimal LeaseLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("遞延所得稅負債"), JsonRequired]
    public decimal DeferredTaxLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("其他負債"), JsonRequired]
    public decimal OtherLiabs { get; init; } = 0.00m;
    [property: JsonPropertyName("負債總計"), JsonRequired]
    public decimal TotalLiabs { get; init; } = 0.00m;

    // 4. 權益類項目
    [property: JsonPropertyName("股本"), JsonRequired]
    public decimal ShareCapital { get; init; } = 0.00m;
    [property: JsonPropertyName("權益─具證券性質之虛擬通貨"), JsonRequired]
    public decimal SecurityTokenEquity { get; init; } = 0.00m;
    [property: JsonPropertyName("資本公積"), JsonRequired]
    public decimal CapitalSurplus { get; init; } = 0.00m;
    [property: JsonPropertyName("保留盈餘"), JsonRequired]
    public decimal RetainedEarnings { get; init; } = 0.00m;
    [property: JsonPropertyName("其他權益"), JsonRequired]
    public decimal OtherEquity { get; init; } = 0.00m;
    [property: JsonPropertyName("庫藏股票"), JsonRequired]
    public decimal TreasuryStock { get; init; } = 0.00m;
    [property: JsonPropertyName("歸屬於母公司業主之權益合計"), JsonRequired]
    public decimal EquityAttributableToOwnersOfParent { get; init; } = 0.00m;
    [property: JsonPropertyName("共同控制下前手權益"), JsonRequired]
    public decimal PredecessorInterests { get; init; } = 0.00m;
    [property: JsonPropertyName("合併前非屬共同控制股權"), JsonRequired]
    public decimal EquityNotUnderCommonControl { get; init; } = 0.00m;
    [property: JsonPropertyName("非控制權益"), JsonRequired]
    public decimal NonControllingInterests { get; init; } = 0.00m;
    [property: JsonPropertyName("權益總計"), JsonRequired]
    public decimal TotalEquity { get; init; } = 0.00m;

    // 5. 保險業特有總計項目
    [property: JsonPropertyName("負債及權益總計"), JsonRequired]
    public decimal TotalLiabsAndEquity { get; init; } = 0.00m;

    // 6. 股數與每股價值項目
    [property: JsonPropertyName("待註銷股本股數（單位：股）"), JsonRequired]
    public decimal PendingCancellationShares { get; init; } = 0.00m;
    [property: JsonPropertyName("預收股款（權益項下）之約當發行股數（單位：股）"), JsonRequired]
    public decimal PreReceivedSharesEquivalent { get; init; } = 0.00m;
    [property: JsonPropertyName("母公司暨子公司所持有之母公司庫藏股股數（單位：股）"), JsonRequired]
    public decimal ParentSubsidiaryTreasuryShares { get; init; } = 0.00m;
    [property: JsonPropertyName("每股參考淨值"), JsonRequired]
    public decimal NetValuePerShare { get; init; } = 0.00m;

    // 7. 系統稽核欄位
    public DateTime? UpdatedAt = null;
}