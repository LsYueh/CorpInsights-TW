namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公發公司資產負債表-保險業
/// </summary>
public record InsDto(
    // 1. 核心識別與索引欄位
    short Year,
    byte Quarter,
    string MarketType,
    string CompanyCode,
    string CompanyName,

    // 2. 保險業資產類項目
    decimal CashAndCashEquivalents = 0.00m,
    decimal Receivables = 0.00m,
    decimal CurrentTaxAssets = 0.00m,
    decimal AssetsHeldForSale = 0.00m,
    decimal AssetsForDistribution = 0.00m,
    decimal Investments = 0.00m,
    decimal ReinsuranceContractAssets = 0.00m,
    decimal PropertyAndEquipment = 0.00m,
    decimal RightOfUseAssets = 0.00m,
    decimal IntangibleAssets = 0.00m,
    decimal DeferredTaxAssets = 0.00m,
    decimal OtherAssets = 0.00m,
    decimal AssetsOnSeparateAccounts = 0.00m,
    decimal TotalAssets = 0.00m,

    // 3. 保險業負債類項目
    decimal ShortTermDebts = 0.00m,
    decimal Payables = 0.00m,
    decimal CurrentTaxLiabs = 0.00m,
    decimal LiabsRelatedToAssetsHeldForSale = 0.00m,
    decimal FinancialLiabsAtFvtpl = 0.00m,
    decimal DerivativeFinancialLiabsForHedging = 0.00m,
    decimal BondsPayable = 0.00m,
    decimal PreferredStockLiabs = 0.00m,
    decimal OtherFinancialLiabs = 0.00m,
    decimal LeaseLiabs = 0.00m,
    decimal InsuranceLiabs = 0.00m,
    decimal FinancialInsuranceReserves = 0.00m,
    decimal ForeignExchangeValuationReserves = 0.00m,
    decimal Provisions = 0.00m,
    decimal DeferredTaxLiabs = 0.00m,
    decimal OtherLiabs = 0.00m,
    decimal LiabsOnSegregatedAccounts = 0.00m,
    decimal TotalLiabs = 0.00m,

    // 4. 權益類項目
    decimal ShareCapital = 0.00m,
    decimal SecurityTokenEquity = 0.00m,
    decimal CapitalSurplus = 0.00m,
    decimal RetainedEarnings = 0.00m,
    decimal OtherEquity = 0.00m,
    decimal TreasuryStock = 0.00m,
    decimal EquityAttributableToOwnersOfParent = 0.00m,
    decimal PredecessorInterests = 0.00m,
    decimal EquityNotUnderCommonControl = 0.00m,
    decimal NonControllingInterests = 0.00m,
    decimal TotalEquity = 0.00m,

    // 5. 保險業特有總計項目
    decimal TotalLiabsAndEquity = 0.00m,

    // 6. 股數與每股價值項目
    decimal PendingCancellationShares = 0.00m,
    decimal PreReceivedSharesEquivalent = 0.00m,
    decimal ParentSubsidiaryTreasuryShares = 0.00m,
    decimal NetValuePerShare = 0.00m,

    // 7. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto;