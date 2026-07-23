using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Core.Storage;

/// <summary>
/// 
/// </summary>
public record StorageContext(
    T187ApCode ApCode,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    DateOnly? Date = null
);