using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.DataFetcher.Core.Common;

/// <summary>
/// 
/// </summary>
public record FetchContext(
    StockMarket Market,
    StatementType ApCode,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy
);