using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Etl;

public record RuntimeConfig(
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    T187ApCode ApCode,
    DateOnly Date,
    bool IsDryRun);