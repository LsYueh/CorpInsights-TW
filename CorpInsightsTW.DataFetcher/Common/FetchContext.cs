using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.DataFetcher.Common;

/// <summary>
/// 
/// </summary>
public record FetchContext(
    T187ApCode ApCode,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy
);