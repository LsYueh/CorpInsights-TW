namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公發公司資產負債表-金控業
/// </summary>
public record FhDto(
    // 1. 核心識別與索引欄位
    short Year,
    byte Quarter,
    string MarketType,
    string CompanyCode,
    string CompanyName,

    // 2. 金控業資產類項目
    decimal CashAndCashEquivalents = 0.00m,
    decimal DueFromCentralBankAndFinancialPeers = 0.00m,
    decimal FinancialAssetsAtFvtpl = 0.00m,
    decimal FinancialAssetsAtFvoci = 0.00m,
    decimal FinancialAssetsAtAmortizedCost = 0.00m,
    decimal DerivativeFinancialAssetsForHedging = 0.00m,
    decimal ReverseRepo = 0.00m,
    decimal Receivables = 0.00m,
    decimal CurrentTaxAssets = 0.00m,
    decimal AssetsHeldForSale = 0.00m,
    decimal AssetsForDistribution = 0.00m,
    decimal DiscountsAndLoans = 0.00m,
    decimal ReinsuranceContractAssets = 0.00m,
    decimal InvestmentsUsingEquityMethod = 0.00m,
    decimal RestrictedAssets = 0.00m,
    decimal OtherFinancialAssets = 0.00m,
    decimal InvestmentProperty = 0.00m,
    decimal PropertyAndEquipment = 0.00m,
    decimal RightOfUseAssets = 0.00m,
    decimal IntangibleAssets = 0.00m,
    decimal DeferredTaxAssets = 0.00m,
    decimal OtherAssets = 0.00m,
    decimal TotalAssetsAmount = 0.00m,

    // 3. 金控業負債類項目
    decimal DepositsFromCentralBankAndFinancialPeers = 0.00m,
    decimal DueToCentralBankAndFinancialPeers = 0.00m,
    decimal FinancialLiabsAtFvtpl = 0.00m,
    decimal DerivativeFinancialLiabsForHedgingNet = 0.00m,
    decimal RepoLiabs = 0.00m,
    decimal CommercialPaperPayableNet = 0.00m,
    decimal Payables = 0.00m,
    decimal CurrentTaxLiabs = 0.00m,
    decimal LiabsRelatedToAssetsHeldForSale = 0.00m,
    decimal DepositsAndRemittances = 0.00m,
    decimal BondsPayable = 0.00m,
    decimal OtherBorrowings = 0.00m,
    decimal PreferredStockLiabs = 0.00m,
    decimal ProvisionsForLiabs = 0.00m,
    decimal OtherFinancialLiabs = 0.00m,
    decimal LeaseLiabs = 0.00m,
    decimal DeferredTaxLiabs = 0.00m,
    decimal OtherLiabs = 0.00m,
    decimal TotalLiabsAmount = 0.00m,

    // 4. 權益類項目
    decimal ShareCapital = 0.00m,
    decimal CapitalSurplus = 0.00m,
    decimal RetainedEarnings = 0.00m,
    decimal OtherEquity = 0.00m,
    decimal TreasuryStock = 0.00m,
    decimal EquityAttributableToOwnersOfParent = 0.00m,
    decimal PredecessorInterests = 0.00m,
    decimal NonControllingInterests = 0.00m,
    decimal TotalEquityAmount = 0.00m,

    // 5. 股數與每股價值項目
    decimal PendingCancellationShares = 0.00m,
    decimal PreReceivedSharesEquivalent = 0.00m,
    decimal ParentSubsidiaryTreasuryShares = 0.00m,
    decimal NetValuePerShare = 0.00m,

    // 6. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto;