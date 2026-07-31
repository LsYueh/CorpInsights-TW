using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.DataFetcher.Core.Common;

/// <summary>
/// 
/// </summary>
public record FetchContext(
    StockMarket Market,
    StatementType Type,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy
);