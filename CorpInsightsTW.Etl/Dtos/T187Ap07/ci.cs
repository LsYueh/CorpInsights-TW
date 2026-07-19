namespace CorpInsightsTW.Etl.Dtos.T187Ap07;

/// <summary>
/// 公發公司資產負債表-一般業
/// </summary>
public record CiDto(
    // 1. 核心識別與索引欄位
    short Year,
    byte Quarter,
    string MarketType,
    string CompanyCode,
    string CompanyName,

    // 2. 資產類項目
    decimal CurrentAssets = 0.00m,
    decimal NonCurrentAssets = 0.00m,
    decimal TotalAssets = 0.00m,

    // 3. 負債類項目
    decimal CurrentLiabs = 0.00m,
    decimal NonCurrentLiabs = 0.00m,
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

    // 5. 股數與每股價值項目
    decimal PendingCancellationShares = 0.00m,
    decimal PreReceivedSharesEquivalent = 0.00m,
    decimal ParentSubsidiaryTreasuryShares = 0.00m,
    decimal NetValuePerShare = 0.00m,

    // 6. 系統稽核欄位
    DateTime? UpdatedAt = null
) : IT187RawDto;