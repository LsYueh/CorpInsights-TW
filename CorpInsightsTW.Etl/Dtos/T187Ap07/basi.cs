using System.Text.Json.Serialization;
using CorpInsightsTW.Etl.Core.Json;

namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公司資產負債表-金融業
/// </summary>
public record BasiDto(
    // 1. 核心識別與索引欄位
    [property: JsonPropertyName("年度"), JsonRequired]
    short Year,
    [property: JsonPropertyName("季別"), JsonRequired]
    byte Quarter,
    [property: JsonPropertyName("公司代號"), JsonRequired]
    string CompanyCode,
    [property: JsonPropertyName("公司名稱"), JsonRequired]
    string CompanyName,

    // 2. 金融業資產類項目
    [property: JsonPropertyName("現金及約當現金"), JsonRequired]
    decimal CashAndCashEquivalents = 0.00m,
    [property: JsonPropertyName("存放央行及拆借銀行同業"), JsonRequired]
    decimal DueFromCentralBankAndBanks = 0.00m,
    [property: JsonPropertyName("透過損益按公允價值衡量之金融資產"), JsonRequired]
    decimal FinancialAssetsAtFvtpl = 0.00m,
    [property: JsonPropertyName("透過其他綜合損益按公允價值衡量之金融資產"), JsonRequired]
    decimal FinancialAssetsAtFvoci = 0.00m,
    [property: JsonPropertyName("按攤銷後成本衡量之債務工具投資"), JsonRequired]
    decimal FinancialAssetsAtAmortizedCost = 0.00m,
    [property: JsonPropertyNames("避險之金融資產－淨額", "避險之衍生金融資產淨額"), JsonRequired]
    decimal DerivativeFinancialAssetsForHedging = 0.00m,
    [property: JsonPropertyName("附賣回票券及債券投資淨額"), JsonRequired]
    decimal ReverseRepo = 0.00m,
    [property: JsonPropertyName("應收款項－淨額"), JsonRequired]
    decimal Receivables = 0.00m,
    [property: JsonPropertyNames("本期所得稅資產", "當期所得稅資產"), JsonRequired]
    decimal CurrentTaxAssets = 0.00m,
    [property: JsonPropertyName("待出售資產－淨額"), JsonRequired]
    decimal AssetsHeldForSale = 0.00m,
    [property: JsonPropertyName("待分配予業主之資產－淨額"), JsonRequired]
    decimal AssetsForDistribution = 0.00m,
    [property: JsonPropertyName("貼現及放款－淨額"), JsonRequired]
    decimal DiscountsAndLoans = 0.00m,
    [property: JsonPropertyName("採用權益法之投資－淨額"), JsonRequired]
    decimal InvestmentsUsingEquityMethod = 0.00m,
    [property: JsonPropertyName("受限制資產－淨額"), JsonRequired]
    decimal RestrictedAssets = 0.00m,
    [property: JsonPropertyName("其他金融資產－淨額"), JsonRequired]
    decimal OtherFinancialAssets = 0.00m,
    [property: JsonPropertyName("不動產及設備－淨額"), JsonRequired]
    decimal PropertyAndEquipment = 0.00m,
    [property: JsonPropertyName("使用權資產－淨額"), JsonRequired]
    decimal RightOfUseAssets = 0.00m,
    [property: JsonPropertyNames("投資性不動產－淨額", "投資性不動產投資－淨額"), JsonRequired]
    decimal InvestmentProperty = 0.00m,
    [property: JsonPropertyName("無形資產－淨額"), JsonRequired]
    decimal IntangibleAssets = 0.00m,
    [property: JsonPropertyName("遞延所得稅資產"), JsonRequired]
    decimal DeferredTaxAssets = 0.00m,
    [property: JsonPropertyName("其他資產－淨額"), JsonRequired]
    decimal OtherAssets = 0.00m,
    [property: JsonPropertyName("資產總計"), JsonRequired]
    decimal TotalAssetsAmount = 0.00m,

    // 3. 金融業負債類項目
    [property: JsonPropertyName("央行及銀行同業存款"), JsonRequired]
    decimal DepositsFromCentralBankAndBanks = 0.00m,
    [property: JsonPropertyName("央行及同業融資"), JsonRequired]
    decimal DueToCentralBankAndBanks = 0.00m,
    [property: JsonPropertyName("透過損益按公允價值衡量之金融負債"), JsonRequired]
    decimal FinancialLiabsAtFvtpl = 0.00m,
    [property: JsonPropertyNames("避險之金融負債", "避險之衍生金融負債－淨額"), JsonRequired]
    decimal DerivativeFinancialLiabsForHedging = 0.00m,
    [property: JsonPropertyName("附買回票券及債券負債"), JsonRequired]
    decimal RepoLiabs = 0.00m,
    [property: JsonPropertyName("應付款項"), JsonRequired]
    decimal Payables = 0.00m,
    [property: JsonPropertyName("本期所得稅負債"), JsonRequired]
    decimal CurrentTaxLiabs = 0.00m,
    [property: JsonPropertyName("與待出售資產直接相關之負債"), JsonRequired]
    decimal LiabsRelatedToAssetsHeldForSale = 0.00m,
    [property: JsonPropertyName("存款及匯款"), JsonRequired]
    decimal DepositsAndRemittances = 0.00m,
    [property: JsonPropertyName("應付金融債券"), JsonRequired]
    decimal FinancialBondsPayable = 0.00m,
    [property: JsonPropertyName("應付公司債"), JsonRequired]
    decimal BondsPayable = 0.00m,
    [property: JsonPropertyName("特別股負債"), JsonRequired]
    decimal PreferredStockLiabs = 0.00m,
    [property: JsonPropertyName("其他金融負債"), JsonRequired]
    decimal OtherFinancialLiabs = 0.00m,
    [property: JsonPropertyName("負債準備"), JsonRequired]
    decimal ProvisionsForLiabs = 0.00m,
    [property: JsonPropertyName("租賃負債"), JsonRequired]
    decimal LeaseLiabs = 0.00m,
    [property: JsonPropertyName("遞延所得稅負債"), JsonRequired]
    decimal DeferredTaxLiabs = 0.00m,
    [property: JsonPropertyName("其他負債"), JsonRequired]
    decimal OtherLiabs = 0.00m,
    [property: JsonPropertyName("負債總計"), JsonRequired]
    decimal TotalLiabsAmount = 0.00m,

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
    [property: JsonPropertyNames("權益總計", "權益總額"), JsonRequired]
    decimal TotalEquityAmount = 0.00m,

    // 5. 股數與每股價值項目
    [property: JsonPropertyName("待註銷股本股數（單位：股）"), JsonRequired]
    decimal PendingCancellationShares = 0.00m,
    [property: JsonPropertyName("預收股款（權益項下）之約當發行股數（單位：股）"), JsonRequired]
    decimal PreReceivedSharesEquivalent = 0.00m,
    [property: JsonPropertyName("母公司暨子公司所持有之母公司庫藏股股數（單位：股）"), JsonRequired]
    decimal ParentSubsidiaryTreasuryShares = 0.00m,
    [property: JsonPropertyName("每股參考淨值"), JsonRequired]
    decimal NetValuePerShare = 0.00m,

    // 6. 系統稽核欄位 (由資料庫自動處理，但 DTO 預留以利追蹤，可為 null)
    DateTime? UpdatedAt = null
) : IT187RawDto
{
    public string ListingStatus { get; set; } = string.Empty;
}